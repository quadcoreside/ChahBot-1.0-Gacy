<?php

class Controller
{
	private static $instance;

	public $app;
	public $status_code = 200;
	private $response = array();
	public $Encrypt = true;

	public function __construct($app) {
		self::$instance =& $this;
		$this->app = $app;
		$this->load = new Loader();
		$this->db = &DB();
		
		$this->Lang = new Lang();
		$this->Validator = new Validator($this);
		$this->set('error', false);
	}

	public function set($keys, $value = null) {
		if (is_array($keys)) {
			$this->response += $keys;
		} else {
			$this->response[$keys] = $value;
		}
	}

	/**
	 * Adding Middle Layer to authenticate every request
	 * Checking if the request has valid api key in the 'Authorization' header
	*/
	public function isAuthBot() {
	    // Getting request headers
	    $headers = apache_request_headers();

	    // Verifying Authorization Header
	    if (isset($headers['Authorization'])) {
	        $api_key = $headers['Authorization'];

	        $bot = $this->db->select('*')->where('api_key', $api_key)->get('bots')->row();
	        // validating api key
	        if (!empty($bot)) {
	        	$this->db->where('id', $bot->id)->update('bots', array('ip' => $this->getIP(), 'last_work' => time()));
	            $this->bot = $bot;
	        } else {
		        $this->stopError('Access Denied. Invalid Api key', 401);
	        }
	    } else {
	        // api key is missing in header
	        $this->stopError('Api key is misssing', 400);
	    }
	}

	public function isAuthApi() {

	    if (isset($_REQUEST['api_key']) && !empty($_REQUEST['api_key'])) {
	    	$api_key = $_REQUEST['api_key'];

	    	$api = $this->db->select('*')->where('api_key', $api_key)->get('apis')->row();
	        // validating api key
	        if (!empty($api)) {
	            $this->db->select('*')->where('api_key', $api_key)->update('apis', array('last_work' => time()));
	            $this->api = $api;
	            $this->api->config = @json_decode($api->configs);
	        } else {
		        $this->stopError('Access Denied. Invalid Api key', 401);
	        }
	    } else {
	        // api key is missing in header
	        $this->stopError('Api key is misssing', 400);
	    }
	}

	public function genRandStr($length = 25) {
	    $characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
	    $charactersLength = strlen($characters);
	    $rString = '';
	    for ($i = 0; $i < $length; $i++) {
	        $rString .= $characters[rand(0, $charactersLength - 1)];
	    }
	    return $rString;
    }

    public function getCountry() {
		$gi = geoip_open(HELPER . DS . 'GeoIP.dat', GEOIP_STANDARD);
		$country = geoip_country_name_by_addr($gi, $this->getIP());
		geoip_close($gi);
		$country = empty($country) ? 'N/A' : $country;
		return $country;
	}
	public function getIP() {
		$client  = @$_SERVER['HTTP_CLIENT_IP'];
		$forward = @$_SERVER['HTTP_X_FORWARDED_FOR'];
		$remote  = $_SERVER['REMOTE_ADDR'];
		if(filter_var($client, FILTER_VALIDATE_IP)){
		  $ip = $client;
		}else if(filter_var($forward, FILTER_VALIDATE_IP)){
		  $ip = $forward;
		}else{
		  $ip = $remote;
		}
		return $ip;
	}

	function request($controller, $action, $params = array()) {
		$controller .= 'Controller';
		require_once ROOTROOT.DS.'controller'.DS.$controller.'.php';
		$c = new $controller();
		call_user_func_array(array($c, $action), $params);
	}

	function stopError($msg, $status_code = null) {
		$this->app->status(!is_null($status_code) ? $status_code : $this->status_code);

		$this->response['error'] = true;
        $this->response['message'] = $msg;

        $this->echoRespnse();
        $this->app->stop();
	}

	/**
	 * Renvoie json reponse au client
	 * @param String $status_code Le code http de la reponse
	 * @param Array $response array sera convertie en Json
	 */
	public function echoRespnse() {
		$this->app->status($this->status_code);

	    // setting response content type to json
	    $this->app->contentType('application/json');
	    $output = array();

	    if ($this->Encrypt) {
	    	$crypter = new CrypterRC4();
	    	$json = json_encode($this->response);
		    $json = $crypter->Encrypt($json, Conf::$encryption_key);
		    $output = array('d' => $json);
	    } else {
	    	$output = $this->response;
	    }
	    
	    echo json_encode($output);
	}

	public static function &get_instance()
	{
		return self::$instance;
	}

}