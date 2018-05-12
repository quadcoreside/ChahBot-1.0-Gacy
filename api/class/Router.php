<?php
/**
* Router
*/
class Router
{
	private $app;

	function __construct($app) {
		$this->app = $app;
	}

	public function call($method, $url, $action) {

		return $this->app->$method($url, function() use ($action){

			//Resolue le nom de controller et action
			$action = explode('@', $action);
			$controller_name = ucfirst(strtolower($action[0])) . 'Controller';
			$method = $action[1];

			//Auto inclusion du controller
			$file = CLASS_ . DS . 'Controller' . DS . $controller_name . '.php';
			if (file_exists($file)) {
				require $file;
			} else {
				$controller = new Controller($this->app);
				$controller->stopError('404 Not found', 404);
			}

			//On le définie et on lui fait passe le relais a la methode appelé du controller
			$controller = new $controller_name($this->app);
			call_user_func_array([$controller, $method], func_get_args());
			$controller->echoRespnse();
		});

	}

	public function get($url, $action) {

		return $this->call('get', $url, $action);
		
	}

	public function post($url, $action) {

		return $this->call('post', $url, $action);

	}

	public function put($url, $action) {

		return $this->call('put', $url, $action);

	}

	public function delete($url, $action) {

		return $this->call('delete', $url, $action);

	}

}