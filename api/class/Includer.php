<?php
	require CLASS_.DS.'Router.php';

	require ROOTROOT.DS.'config'.DS.'config.php';
	require ROOT.DS.'config'.DS.'autoload.php';

	require CLASS_.DS.'Validator.php';
	require CLASS_.DS.'Controller.php';
	require ROOTROOT.DS.'core'.DS.'Base.php';
	require ROOTROOT.DS.'core'.DS.'Loader.php';
	require ROOTROOT.DS.'core'.DS.'Lang.php';
	require ROOTROOT.DS.'core'.DS.'Log.php';
	require ROOTROOT.DS.'core'.DS.'Database/DB.php';

	require(ROOTROOT . DS . 'helpers' . DS .  'rc4.php');
	require(ROOTROOT . DS . 'helpers' . DS .  'geoip.php');