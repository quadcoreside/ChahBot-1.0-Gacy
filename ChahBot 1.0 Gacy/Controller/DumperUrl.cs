using ChahBot_1_0_Gacy.SqliClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChahBot_1_0_Gacy.Controller
{
    class DumperUrl
    {
        private Dumper dumper = new Dumper();
        internal void serverInfo(string url)
        {
            dumper.txtURL = url;
            dumper.__URL = url;
            dumper.__SQLType = DataBase.Types.MySQL_With_Error;
            dumper.rdbMySQLErrorType1 = true;
            dumper.StartWorker(ChahBot_1_0_Gacy.SqliClass.Dumper.enTypeGUI.Info);
        }

        internal void dataBases()
        {
            dumper.StartWorker(ChahBot_1_0_Gacy.SqliClass.Dumper.enTypeGUI.DataBases);
        }

        internal void tables()
        {
            dumper.StartWorker(ChahBot_1_0_Gacy.SqliClass.Dumper.enTypeGUI.Tables);
        }

        internal void Columns()
        {
            dumper.StartWorker(ChahBot_1_0_Gacy.SqliClass.Dumper.enTypeGUI.Columns);
        }

        internal void DumpData()
        {
            //dumper.__DumpToFile = true;
            dumper.StartWorker(ChahBot_1_0_Gacy.SqliClass.Dumper.enTypeGUI.Data);
        }

        internal void DumpCustom()
        {
            dumper.StartWorker(ChahBot_1_0_Gacy.SqliClass.Dumper.enTypeGUI.CustomData);
        }
    }
}
