//using DocumentFormat.OpenXml.Wordprocessing;
//using Microsoft.VisualBasic;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web;

//namespace SCBHarmonization.Helper
//{
//    public class RunPackage
//    {

//        private void MyExtract_Click(System.Object sender, System.EventArgs e)
//        {
//            if (MsgBox("Extracting for :  " + StartDate.Value.Date + "  ", MsgBoxStyle.OkCancel) == MsgBoxResult.Ok)
//            {
//                try
//                {
//                    string logcontent = "Start Time: " + DateTime.Now.ToString() + Constants.vbCrLf;
//                    string dataKount;
//                    Status.Text = logcontent;
//                    // Status.Text = Status.Text & "Establishing connection to client data source ..............."
//                    DataSet SSISExt = new DataSet();


//                    // Status.Text = Status.Text & logcontent
//                    Writelog(logcontent);
//                    _RunningPackage = "DataLoader";  // Package Name
//                                                     // Lets then Go Ahead and Invoke the BackGround Process to Do the Extraction

//                    string msg = "";
//                    DataExtractor h = new DataExtractor();
//                    string sourceLocation = AppConfig.PackagePath.ToString;
//                    dataKount = h.LaunchPackage("dts", sourceLocation, _RunningPackage, StartDate.Value.Date, EndDate.Value.Date, msg);
//                    logcontent = dataKount;
//                    Status.Text = Status.Text + dataKount + Constants.vbCrLf;
//                    logcontent = "End Time: " + DateTime.Now.ToString();
//                    Status.Text = Status.Text + logcontent + Constants.vbCrLf;
//                    // Update Process check list
//                    SqlHelper.ExecuteDataset(AppConfig.ConnectionString, "eFASS_ProcessCheckListsUpdate", 0);
//                    System.Windows.Forms.MessageBox.Show("Data extraction has been successfully completed", "Fintrak");
//                }
//                catch (Exception ex)
//                {
//                    System.Windows.Forms.MessageBox.Show(ex.Message, "Fintrak", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//        }

//    }
//}