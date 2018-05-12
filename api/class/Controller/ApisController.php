<?php
class ApisController extends Controller
{
	
    function run() {
        $this->Encrypt = false;
        $this->isAuthApi();
        $r = array(
            'success' => true
        );

        if (isset($this->api->config->userAgentHook) && $this->api->config->userAgentHook == 'on') {
            if ($this->api->user_agent != '%' && $this->api->user_agent != $_SERVER['HTTP_USER_AGENT']) {
                $this->stopError('Access Denied Code 125', 401);
            }
        }

        switch ($this->api->config->function_index) {
            
            case 1: //DATA SAVE
                $this->Validator->verifyRequiredParams(array($this->api->config->name, $this->api->config->subject, $this->api->config->content));

                $name = $this->app->request->post($this->api->config->name);
                $subject = $this->app->request->post($this->api->config->subject);
                $content = $this->app->request->post($this->api->config->content);

                $this->db->insert('inboxs', array(
                    'user_id' => $this->api->user_id,
                    'api_key' => $this->api->api_key,
                    'name' => $name,
                    'subject' => $subject,
                    'content' => $content,
                    'ip' => $_SERVER['REMOTE_ADDR'],
                    'user_agent' => @$_SERVER['HTTP_USER_AGENT'],
                    'favorite' => 0,
                    'viewed' => 0,
                    'trash' => 0,
                    'date_created' => date('Y-m-d H:i:s')
                ));

                break;
                
            default:
                $r['success'] = false;
                $r['message'] = 'Error read API Config';
                break;
        }
        
        $r['success'] = true;

        $this->set($r);
    }


}