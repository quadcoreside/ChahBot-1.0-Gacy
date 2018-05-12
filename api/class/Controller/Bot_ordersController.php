<?php
class Bot_ordersController extends Controller
{
    function getProgramedOrder($bot_id) { 
        $programs = $this->db->select('*')->where('bot_id', $bot_id)->or_where('bot_id', 0)->get('bot_programs')->result();
        $order = new StdClass();

        foreach ($programs as $k => $v) {
            switch ($v->action) {
                case 'start_search':

                    if ($v->last_time < ($v->repeat_time - time())) {
                        if ($conf = @json_decode($v->content)) {

                            if ($conf->genDorks == 1) {
                                $dorks = $this->db->query('SELECT value FROM dorks ORDER BY RAND() LIMIT ' . $conf->limitDorks)->result();
                                $dorks_array = array();
                                foreach ($dorks as $k => $v) {
                                    $dorks_array[] = $v->value;
                                }
                                $order->dorks = implode("\n", $dorks_array);
                            }
                            unset($conf->genDorks);

                            $order = $conf;
                            if ($tasks->loop == 1) {
                                $this->db->update('bot_programs', array('last_time' => time()));
                            } else {
                                $this->db->delete('bot_programs', array('id' => $v->id));
                            }

                            return $order;
                        }
                    }

                    break;

                default:
                    if ($v->last_time < ($v->repeat_time - time())) {
                        if ($conf = @json_decode($v->content)) {

                            $order = $conf;
                            if ($tasks->loop == 1) {
                                $this->db->update('bot_programs', array('last_time' => time()));
                            } else {
                                $this->db->delete('bot_programs', array('id' => $v->id));
                            }

                            return $order;
                        }
                    }
                    break;
            }
        }
        
        return false;
    }

    function get() {
        $this->isAuthBot();
        $r = array(
            'success' => true,
            'available' => true
        );
        
        $task = $this->db->select('*')->where('bot_id', $this->bot->id)->or_where('bot_id', 0)->get('bot_orders')->row();

        if (!empty($task)) {
			$this->db->delete('bot_orders', array('id' => $task->id));

            if ($order = @json_decode($task->content)) {
                unset($task->content);
                $order->id = $task->id;

                $r['id'] = $task->id;
                $r['order'] = $order;
                $r['date_created'] = $task->date_created;
            } else {
                $r['message'] = 'Error decode Json';
                $r['available'] = false;
            }
        } else {
            if ($order = $this->getProgramedOrder($this->bot->id)) {
                $r['id'] = random_string('alnum', 5);
                $order->id = $r['id'];
                $r['order'] = $order;
                $r['date_created'] = $task->date_created;
            } else {
                $r['available'] = false;
            }
        }

        $this->set($r);
    }

    function update_status() {
        $this->isAuthBot();
        $this->Validator->verifyRequiredParams(array('order_id', 'name', 'status'));
        $r = array(
            'success' => true
        );

        $order_id = $this->app->request->post('order_id');
        $name = $this->app->request->post('name') . ' ' . $this->bot->name;
        $status = $this->app->request->post('status');
        $pourcentage = $this->app->request->post('pourcentage');

        $hash = md5('bot' . $this->bot->id . $order_id);

        $task = $this->db->select('*')->where('hash', $hash)->get('tasks')->row();

        $bar_class = 'text-aqua';

        if (empty($task)) {

            $this->db->insert('tasks', array(
                'hash' => $hash,
                'name' => $name,
                'status' => $status,
                'bar_class' => $bar_class,
                'date_created' => date('Y-m-d H:i:s')
            ));
        } else {
            $this->db->where('id', $task->id)->update('tasks', array(
                'hash' => $hash,
                'name' => $name,
                'status' => $status,
                'bar_class' => $bar_class,
                'date_created' => date('Y-m-d H:i:s')
            ));
        }

        

        $this->status_code = 201;
        $this->set($r);
    }

    function update_progress() {
        $this->isAuthBot();
        $this->Validator->verifyRequiredParams(array('order_id', 'pourcentage'));
        $r = array(
            'success' => true
        );

        $order_id = $this->app->request->post('order_id');
        $pourcentage = $this->app->request->post('pourcentage');

        $hash = md5('bot' . $this->bot->id . $order_id);

        $task = $this->db->select('*')->where('hash', $hash)->get('tasks')->row();

        if (empty($task)) {
            $this->db->insert('tasks', array(
                'hash' => $hash,
                'pourcentage' => $pourcentage,
                'date_created' => date('Y-m-d H:i:s')
            ));
        } else {
            $this->db->where('id', $task->id)->update('tasks', array(
                'hash' => $hash,
                'pourcentage' => $pourcentage,
            ));
        }

        $this->status_code = 201;
        $this->set($r);
    }

}