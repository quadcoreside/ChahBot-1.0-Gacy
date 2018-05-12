<?php
	
	define('PUBLIC_', dirname(__FILE__));
	define('ROOT', dirname(PUBLIC_));
	define('ROOTROOT', dirname(ROOT));
	define('DS', DIRECTORY_SEPARATOR);
	define('CLASS_', ROOT.DS.'class');
	define('CORE', ROOTROOT.DS.'core');
	define('HELPER', ROOTROOT.DS.'helpers');
	define('DATA', ROOTROOT.DS.'datas');

	require CLASS_.DS.'Includer.php';
	require CLASS_.DS.'vendor'.DS.'autoload.php';
	
	$app = new \Slim\Slim(array(
	    'mode' => 'development'
	));

	// Only invoked if mode is "development"
	/*$app->configureMode('production', function () use ($app) {
		$app->config(array(
			'log.enable' => true,
			'debug' => false
		));
	});*/

	// Only invoked if mode is "development"
	$app->configureMode('development', function () use ($app) {
		$app->config(array(
			'log.enable' => false,
			'debug' => true
		));
	});
	date_default_timezone_set('Europe/Paris');

	$router = new Router($app);

	/****************************** Bots *********************************/
	$router->get('/order/get', 'Bot_orders@get')->name('task');
	$router->post('/order/status', 'Bot_orders@update_status')->name('status-task');
	$router->post('/order/progress', 'Bot_orders@update_progress')->name('progress-task');

	$router->get('/sqli/urls', 'Sqli@get_urls')->name('urls');
	$router->get('/sqli/urls_queue', 'Sqli@get_urls_queue')->name('urls');
	$router->get('/sqli/exploitables', 'Sqli@get_exploitables')->name('exploitables');

	$router->post('/sqli/urls', 'Sqli@post_urls')->name('urls');
	$router->post('/sqli/exploitables', 'Sqli@post_exploitables')->name('exploitables');
	
	$router->post('/sqli/injectables', 'Sqli@post_injectables')->name('injectables');
	$router->post('/sqli/injectable', 'Sqli@post_injectable')->name('injectable');

	$router->post('/sqli/non_injectables', 'Sqli@post_non_injectables')->name('non_injectables');
	$router->post('/sqli/non_injectable', 'Sqli@post_non_injectable')->name('non_injectables');


	$router->get('/proxy/get', 'Proxy@get_list')->name('get_proxy_list');
	$router->post('/proxy/post', 'Proxy@add')->name('add_proxy');
	$router->post('/proxy/set_status', 'Proxy@set_status')->name('set_status');

	/****************************** User API *********************************/
	$router->get('/mine', 'Apis@run')->name('mine');
	$router->post('/mine', 'Apis@run')->name('mine');

	$app->run();

?>










