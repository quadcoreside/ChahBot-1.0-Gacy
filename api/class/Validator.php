<?php 
/**
* Field validator
*/
class Validator
{
	protected $controller;

	function __construct($controller) {
		$this->controller = $controller;
	}

	function verifyRequiredParams($required_fields) {
	    $error = false;
	    $error_fields = '';
	    $request_params = array();
	    $request_params = $_REQUEST;

	    // Handling PUT request params
	    if ($_SERVER['REQUEST_METHOD'] == 'PUT') {
	        parse_str($this->controller->app->request()->getBody(), $request_params);
	    }

	    foreach ($required_fields as $field) {
	        if (!isset($request_params[$field]) || strlen(trim($request_params[$field])) <= 0) {
	            $error = true;
	            $error_fields .= $field . ', ';
	        }
	    }

	    if ($error) {
	        //Necésite des champ qui sont absent ou vide stop le processus et declenche une erreur
	        $this->controller->stopError('Required field(s) ' . substr($error_fields, 0, -2) . ' is missing or empty');
	    }
	}

	function validateEmail($email) {
	    if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
	        $response["error"] = true;
	        $response["message"] = 'Email address is not valid';
	        echoRespnse(400, $response);
	        $this->controller->app->stop();
	    }
	}


}


