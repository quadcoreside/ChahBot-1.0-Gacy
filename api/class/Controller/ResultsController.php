<?php
/**
* ResultsController
*/
class ResultsController extends Controller
{
	
    function post() {
        $this->isAuthMachine();
        $this->Validator->verifyRequiredParams(array('admin_id', 'machine_id', 'content'));
        $r = array();

        $admin_id = $this->app->request->post('admin_id');
        $machine_id = $this->app->request->post('machine_id');
        $content = $this->app->request->post('content');

        $this->db->insert('machine_results', array(
            'admin_id' => $admin_id,
            'machine_id' => $this->machine->id,
            'content' => $content
        ));

        $r['success'] = true;

        $this->status_code = 201;
        $this->set($r);
    }

    function put($id) {
        $this->isAuthMachine();
        $this->Validator->verifyRequiredParams(array('admin_id', 'machine_id', 'content'));
        $r = array();

        $admin_id = $this->app->request->post('admin_id');
        $machine_id = $this->app->request->post('machine_id');
        $content = $this->app->request->post('content');

        $this->db->where('id', $id)->update('machine_results', array(
            'admin_id' => $admin_id,
            'machine_id' => $this->machine->id,
            'content' => $content
        ));
        $r['success'] = true;

        $this->status_code = 200;
        $this->set($r);
    }

    /***************************** Admin **********************************/

    function get($machine_id = null) {
        $this->isAuthAdmin();
        $this->loadModel('Result');

        $results = $this->db->select('*')->where('admin_id', $this->admin->id)->where('machine_id', $machine_id)->get('machine_results')->result();
        
        $r['success'] = true;
        $r['results'] = $results;

        $this->set($d);
    }

    
    function gets() {
        $this->isAuthAdmin();
        $this->loadModel('Result');

        $results = $this->db->select('*')->where('admin_id', $this->admin->id)->get('machine_results')->result();
        
        $r['success'] = true;
        $r['results'] = $results;

        $this->set($d);
        
    }

    function delete($id) {
        $this->isAuthAdmin();
        $this->db->delete('machine_results', array('id' => $id));
    }

}