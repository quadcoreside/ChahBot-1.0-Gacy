<?php
class ProxyController extends Controller
{
    function get_list() {
        $this->isAuthBot();
        $r = array(
            'success' => true
        );
        $max = 500;
        if (isset($_GET['max']) && is_numeric($_GET['max'])) {
            $max = intval($_GET['max']);
        }
        
        $proxy = $this->db->query("SELECT ip,port FROM proxy WHERE live = 0 OR live = 1 OR live = 2 limit " . $max)->result();

        $r['proxy'] = $proxy;
        $r['count'] = count($proxy);

        $this->set($r);
    }

    function add() {
        $this->isAuthBot();
        $this->Validator->verifyRequiredParams(array('d'));
        $r = array(
            'success' => true
        );
        ignore_user_abort(true);
        set_time_limit(0);

        $data_crypted = $this->app->request->post('d');
        $crypter = new CrypterRC4();
        $json = $crypter->Decrypt($data_crypted, Conf::$encryption_key);
        $data = json_decode($json);

        $date = date('Y-m-d H:i:s');
        $gi = geoip_open(ROOTROOT . DS . 'helper' . DS . 'GeoIP.dat', GEOIP_STANDARD);

        foreach ($data->proxy as $k => $v) {
            list($ip, $port) = explode(':', $v);
            $this->db->insert('proxy', array(
                'ip' => $ip,
                'port' => $port,
                'country' => geoip_country_name_by_addr($gi, $ip),
                'country_code' => geoip_country_code_by_addr($gi, $ip),
                'time_checked' => '',
                'date_created' => $date
            ));
        }
        geoip_close($gi);

        $this->db->query("DELETE proxy FROM proxy LEFT OUTER JOIN (SELECT MIN(id) as id, url FROM proxy GROUP BY ip, port) as KeepRows ON proxy.id = KeepRows.id WHERE KeepRows.id IS NULL");
        $this->status_code = 201;
        $this->set($r);
    }

    function set_status() {
        $this->isAuthBot();
        $this->Validator->verifyRequiredParams(array('d'));
        $r = array(
            'success' => true
        );

        $data_crypted = $this->app->request->post('d');
        $crypter = new CrypterRC4();
        $json = $crypter->Decrypt($data_crypted, Conf::$encryption_key);
        $data = json_decode($json);

        $proxy = $this->db->select('*')->where('ip', $data->ip)->where('port', $data->port)->get('proxy')->row();

        if (!empty($proxy)) {
            $live = ($data->status == true) ? 1 : 0;
            $this->db->where('id', $proxy->id)->update('proxy', array('live' => $live, 'time_checked' => time()));
        }

        $this->db->query("DELETE proxy FROM proxy LEFT OUTER JOIN (SELECT MIN(id) as id, url FROM proxy GROUP BY ip, port) as KeepRows ON proxy.id = KeepRows.id WHERE KeepRows.id IS NULL");
        $this->status_code = 201;
        $this->set($r);
    }
}

