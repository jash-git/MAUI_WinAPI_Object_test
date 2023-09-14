using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VPOS
{
    //---
    //payment_module資料表內的參數
    /*
    {
	    "server_info":{
		    "production":{
			    "ip":"cmas-gageway.easycard.com.tw",
			    "port":7100
		    },
		    "sandbox":{
			    "ip":"211.78.134.165",
			    "port":7100
		    }
	    }
    }
    */
    public class ECMProduction//產品(正式)
    {
        public string ip { get; set; }
        public int port { get; set; }
    }

    public class ECMSandbox//沙盒(測試) / env_type=T
    {
        public string ip { get; set; }
        public int port { get; set; }
    }

    public class ECMServerInfo
    {
        public ECMProduction production { get; set; }
        public ECMSandbox sandbox { get; set; }
    }

    public class EASY_CARDModule
    {
        public ECMServerInfo server_info { get; set; }
    }
    //---payment_module資料表內的參數

    /*
    //payment_module_params資料表內的參數
    //TOOL: https://json2csharp.com/
    {
	    "env_type": "T",
	    "cmas_mode": "0",
	    "cmas_ip": null,
	    "newspid": "0000000001",
	    "tmlocationid": "0000000001"
    } 
    */
    public class EASY_CARDModuleParams
    {
        public string env_type { get; set; }
        public string cmas_mode { get; set; }
        public object cmas_ip { get; set; }
        public string newspid { get; set; }
        public string tmlocationid { get; set; }
    }

    //------------------------------------------------------

    /*
    //ICERINI.xml 內容
    //TOOL: https://json2csharp.com/code-converters/xml-to-csharp
    <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
    <TransXML>
      <LogFlag>1</LogFlag>
      <DLLVersion>2</DLLVersion>
      <TCPIPTimeOut>10</TCPIPTimeOut>
      <LogCnt>30</LogCnt>
      <ComPort>3</ComPort>
      <ECC_IP>211.78.134.165</ECC_IP>
      <ECC_Port>7100</ECC_Port>
      <ICER_IP>211.78.134.165</ICER_IP>
      <ICER_Port>7100</ICER_Port>
      <CMAS_IP>211.78.134.165</CMAS_IP>
      <CMAS_Port>7100</CMAS_Port>
      <TMLocationID>0000000001</TMLocationID>
      <TMID>01</TMID>
      <TMAgentNumber>0001</TMAgentNumber>
      <LocationID>0</LocationID>
      <NewLocationID>0</NewLocationID>
      <SPID>0</SPID>
      <NewSPID>0</NewSPID>
      <Slot>11</Slot>
      <BaudRate>115200</BaudRate>
      <OpenCom>3</OpenCom>
      <MustSettleDate>10</MustSettleDate>
      <ReaderMode>1</ReaderMode>
      <BatchFlag>1</BatchFlag>
      <OnlineFlag>1</OnlineFlag>
      <ICERDataFlag>1</ICERDataFlag>
      <MessageHeader>99909020</MessageHeader>
      <DLLMode>0</DLLMode>
      <AutoLoadMode>1</AutoLoadMode>
      <MaxALAmt>1000</MaxALAmt>
      <Dev_Info>1122334455</Dev_Info>
      <TCPIP_SSL>1</TCPIP_SSL>
      <CMASAdviceVerify>0</CMASAdviceVerify>
      <AutoSignOnPercnet>0</AutoSignOnPercnet>
      <AutoLoadFunction>0</AutoLoadFunction>
      <VerificationCode>0</VerificationCode>
      <ReSendReaderAVR>0</ReSendReaderAVR>
      <XMLHeaderFlag>0</XMLHeaderFlag>
      <FolderCreatFlag>1</FolderCreatFlag>
      <BLCName>BLC03342A_190702.BIG</BLCName>
      <CMASMode>0</CMASMode>
      <POS_ID>0</POS_ID>
      <AdditionalTcpipData>0</AdditionalTcpipData>
      <PacketLenFlag>0</PacketLenFlag>
      <CRT_FileName>
      </CRT_FileName>
      <Key_FileName>
      </Key_FileName>
      <ICERFlowDebug>1</ICERFlowDebug>
      <AdviceFlag>0</AdviceFlag>
      <ReaderPortocol>0</ReaderPortocol>
      <AccFreeRidesMode>0</AccFreeRidesMode>
      <ETxnSignOnMode>0</ETxnSignOnMode>
      <CloseAntenna>0</CloseAntenna>
      <ReaderUartDebug>0</ReaderUartDebug>
      <MerchantID>00000000</MerchantID>
      <ICERQRTxn>1</ICERQRTxn>
      <ICERKey>TestEASYCARDTestEASYCARD99900000</ICERKey>
      <RS232Parameter>8N1</RS232Parameter>
      <SignOnMode>0</SignOnMode>
      <ReadAccPointsFlag>0</ReadAccPointsFlag>
      <ReadAccPointsMode>0</ReadAccPointsMode>
      <ReadDateOfFirstTransFlag>0</ReadDateOfFirstTransFlag>
      <GroupFlag>1</GroupFlag>
      <CommandMode>0</CommandMode>
    </TransXML>
    */

    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(TransXML));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (TransXML)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "TransXML")]
    public class TransXML
    {

        [XmlElement(ElementName = "LogFlag")]
        public int LogFlag { get; set; }

        [XmlElement(ElementName = "DLLVersion")]
        public int DLLVersion { get; set; }

        [XmlElement(ElementName = "TCPIPTimeOut")]
        public int TCPIPTimeOut { get; set; }

        [XmlElement(ElementName = "LogCnt")]
        public int LogCnt { get; set; }

        [XmlElement(ElementName = "ComPort")]
        public int ComPort { get; set; }

        [XmlElement(ElementName = "ECC_IP")]
        public string ECCIP { get; set; }

        [XmlElement(ElementName = "ECC_Port")]
        public int ECCPort { get; set; }

        [XmlElement(ElementName = "ICER_IP")]
        public string ICERIP { get; set; }

        [XmlElement(ElementName = "ICER_Port")]
        public int ICERPort { get; set; }

        [XmlElement(ElementName = "CMAS_IP")]
        public string CMASIP { get; set; }

        [XmlElement(ElementName = "CMAS_Port")]
        public int CMASPort { get; set; }

        [XmlElement(ElementName = "TMLocationID")]
        public string TMLocationID { get; set; }

        [XmlElement(ElementName = "TMID")]
        public string TMID { get; set; }

        [XmlElement(ElementName = "TMAgentNumber")]
        public string TMAgentNumber { get; set; }

        [XmlElement(ElementName = "LocationID")]
        public int LocationID { get; set; }

        [XmlElement(ElementName = "NewLocationID")]
        public int NewLocationID { get; set; }

        [XmlElement(ElementName = "SPID")]
        public int SPID { get; set; }

        [XmlElement(ElementName = "NewSPID")]
        public string NewSPID { get; set; }

        [XmlElement(ElementName = "Slot")]
        public int Slot { get; set; }

        [XmlElement(ElementName = "BaudRate")]
        public int BaudRate { get; set; }

        [XmlElement(ElementName = "OpenCom")]
        public int OpenCom { get; set; }

        [XmlElement(ElementName = "MustSettleDate")]
        public int MustSettleDate { get; set; }

        [XmlElement(ElementName = "ReaderMode")]
        public int ReaderMode { get; set; }

        [XmlElement(ElementName = "BatchFlag")]
        public int BatchFlag { get; set; }

        [XmlElement(ElementName = "OnlineFlag")]
        public int OnlineFlag { get; set; }

        [XmlElement(ElementName = "ICERDataFlag")]
        public int ICERDataFlag { get; set; }

        [XmlElement(ElementName = "MessageHeader")]
        public string MessageHeader { get; set; }

        [XmlElement(ElementName = "DLLMode")]
        public int DLLMode { get; set; }

        [XmlElement(ElementName = "AutoLoadMode")]
        public int AutoLoadMode { get; set; }

        [XmlElement(ElementName = "MaxALAmt")]
        public int MaxALAmt { get; set; }

        [XmlElement(ElementName = "Dev_Info")]
        public string DevInfo { get; set; }

        [XmlElement(ElementName = "TCPIP_SSL")]
        public int TCPIPSSL { get; set; }

        [XmlElement(ElementName = "CMASAdviceVerify")]
        public int CMASAdviceVerify { get; set; }

        [XmlElement(ElementName = "AutoSignOnPercnet")]
        public int AutoSignOnPercnet { get; set; }

        [XmlElement(ElementName = "AutoLoadFunction")]
        public int AutoLoadFunction { get; set; }

        [XmlElement(ElementName = "VerificationCode")]
        public int VerificationCode { get; set; }

        [XmlElement(ElementName = "ReSendReaderAVR")]
        public int ReSendReaderAVR { get; set; }

        [XmlElement(ElementName = "XMLHeaderFlag")]
        public int XMLHeaderFlag { get; set; }

        [XmlElement(ElementName = "FolderCreatFlag")]
        public int FolderCreatFlag { get; set; }

        [XmlElement(ElementName = "BLCName")]
        public string BLCName { get; set; }

        [XmlElement(ElementName = "CMASMode")]
        public int CMASMode { get; set; }

        [XmlElement(ElementName = "POS_ID")]
        public int POSID { get; set; }

        [XmlElement(ElementName = "AdditionalTcpipData")]
        public int AdditionalTcpipData { get; set; }

        [XmlElement(ElementName = "PacketLenFlag")]
        public int PacketLenFlag { get; set; }

        [XmlElement(ElementName = "CRT_FileName")]
        public object CRTFileName { get; set; }

        [XmlElement(ElementName = "Key_FileName")]
        public object KeyFileName { get; set; }

        [XmlElement(ElementName = "ICERFlowDebug")]
        public int ICERFlowDebug { get; set; }

        [XmlElement(ElementName = "AdviceFlag")]
        public int AdviceFlag { get; set; }

        [XmlElement(ElementName = "ReaderPortocol")]
        public int ReaderPortocol { get; set; }

        [XmlElement(ElementName = "AccFreeRidesMode")]
        public int AccFreeRidesMode { get; set; }

        [XmlElement(ElementName = "ETxnSignOnMode")]
        public int ETxnSignOnMode { get; set; }

        [XmlElement(ElementName = "CloseAntenna")]
        public int CloseAntenna { get; set; }

        [XmlElement(ElementName = "ReaderUartDebug")]
        public int ReaderUartDebug { get; set; }

        [XmlElement(ElementName = "MerchantID")]
        public string MerchantID { get; set; }

        [XmlElement(ElementName = "ICERQRTxn")]
        public int ICERQRTxn { get; set; }

        [XmlElement(ElementName = "ICERKey")]
        public string ICERKey { get; set; }

        [XmlElement(ElementName = "RS232Parameter")]
        public string RS232Parameter { get; set; }

        [XmlElement(ElementName = "SignOnMode")]
        public int SignOnMode { get; set; }

        [XmlElement(ElementName = "ReadAccPointsFlag")]
        public int ReadAccPointsFlag { get; set; }

        [XmlElement(ElementName = "ReadAccPointsMode")]
        public int ReadAccPointsMode { get; set; }

        [XmlElement(ElementName = "ReadDateOfFirstTransFlag")]
        public int ReadDateOfFirstTransFlag { get; set; }

        [XmlElement(ElementName = "GroupFlag")]
        public int GroupFlag { get; set; }

        [XmlElement(ElementName = "CommandMode")]
        public int CommandMode { get; set; }

        public TransXML()
        {
            LogFlag = 1;
            DLLVersion = 2;
            TCPIPTimeOut = 10;
            LogCnt = 50;
            ComPort = 3;
            ECCIP = "211.78.134.165";
            ECCPort = 7100;
            ICERIP = "211.78.134.165";
            ICERPort = 7100;
            CMASIP = "211.78.134.165";
            CMASPort = 7100;
            TMLocationID = "0000000001";
            TMID = "01";
            TMAgentNumber = "0001";
            LocationID = 0;
            NewLocationID = 0;
            SPID = 0;
            NewSPID = "0000000001";
            Slot = 11;
            BaudRate = 115200;
            OpenCom = 4;
            MustSettleDate = 10;
            ReaderMode = 1;
            BatchFlag = 1;
            OnlineFlag = 1;
            ICERDataFlag = 1;
            MessageHeader = "99909020";
            DLLMode = 0;
            AutoLoadMode = 1;
            MaxALAmt = 1000;
            DevInfo = "1122334455";
            TCPIPSSL = 1;
            CMASAdviceVerify = 0;
            AutoSignOnPercnet = 0;
            AutoLoadFunction = 0;
            VerificationCode = 0;
            ReSendReaderAVR = 0;
            XMLHeaderFlag = 0;
            FolderCreatFlag = 1;
            BLCName = "BLC03342A_190702.BIG";
            CMASMode = 0;
            POSID = 0;
            AdditionalTcpipData = 0;
            PacketLenFlag = 0;
            CRTFileName = "";
            KeyFileName = "";
            ICERFlowDebug = 1;
            AdviceFlag = 0;
            ReaderPortocol = 0;
            AccFreeRidesMode = 0;
            ETxnSignOnMode = 0;
            CloseAntenna = 0;
            ReaderUartDebug = 0;
            MerchantID = "00000000";
            ICERQRTxn = 1;
            ICERKey = "TestEASYCARDTestEASYCARD99900000";
            RS232Parameter = "8N1";
            SignOnMode = 0;
            ReadAccPointsFlag = 0;
            ReadAccPointsMode = 0;
            ReadDateOfFirstTransFlag = 0;
            GroupFlag = 1;
            CommandMode = 0;
        }
        
    }
}
