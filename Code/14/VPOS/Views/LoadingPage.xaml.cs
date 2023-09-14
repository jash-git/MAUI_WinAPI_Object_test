using CommunityToolkit.Maui.Views;

namespace VPOS.Views;
/*  	
    Building a login flow with .NET MAUI
        https://www.c-sharpcorner.com/blogs/building-a-login-flow-with-net-maui
*/
public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        /*
        if (await isAuthenticated())
        {
            await Shell.Current.GoToAsync("///home");
        }
        else
        {
            await Shell.Current.GoToAsync("login");
        }
        base.OnNavigatedTo(args);
        */
        LogFile.Write("LoadingPage Start");
        FilesInit(true);

        LogFile.Write("LoadingPage End");
        await Task.Delay(2000);
        await this.ShowPopupAsync(new test_PopupPage());//this.ShowPopup(new PopupPage1());
        await Shell.Current.GoToAsync("main");
        base.OnNavigatedTo(args);
    }

    public void FilesInit(bool blnInit)
    {
        LogFile.CleanLog();
        FileLib.DeleteFile("vpos.db-shm");
        FileLib.DeleteFile("vpos.db-wal");
        FileLib.DeleteFile("vtcloud_sync.db-shm");
        FileLib.DeleteFile("vtcloud_sync.db-wal");
        FileLib.DeleteFile("takeaways.db-shm");
        FileLib.DeleteFile("takeaways.db-wal");

        if (blnInit)
        {
            //---
            //SQLITE資料庫使用動態產生
            String StrFileNameBuf00 = LogFile.m_StrSysPath + "vpos_def.db";
            String StrFileNameBuf01 = LogFile.m_StrSysPath + "vpos.db";
            if ((File.Exists(StrFileNameBuf00)) && (!File.Exists(StrFileNameBuf01)))
            {
                File.Copy(StrFileNameBuf00, StrFileNameBuf01, true);
                LogFile.Write("SystemNormal ; vpos.db Init");
            }
            else
            {
                if (!File.Exists(StrFileNameBuf00))
                {
                    LogFile.Write("SystemError ; vpos_def.db missing");
                }

                if (!File.Exists(StrFileNameBuf01))
                {
                    LogFile.Write("SystemError ; vpos.db missing");
                }
            }

            StrFileNameBuf00 = LogFile.m_StrSysPath + "vtcloud_sync.db";
            if (!File.Exists(StrFileNameBuf00))
            {
                SQLDataTableModel.CreateSQLiteDatabase(StrFileNameBuf00);
                string CreateTableString = SQLDataTableModel.VPOSInitialTableSyntax("upload_data");
                SQLDataTableModel.CreateSQLiteTable(StrFileNameBuf00, CreateTableString);//建立資料表程式
                LogFile.Write("SystemNormal ; vtcloud_sync.db Init");
            }
            //---SQLITE資料庫使用動態產生

            SyncDBData.DBStructRegulating();//資料庫(DB) 新增資料表 / 新增(補)欄位 / 調整欄位資料型
        }
    }
    async Task<bool> isAuthenticated()
    {
        
        await Task.Delay(2000);
        var hasAuth = await SecureStorage.GetAsync("hasAuth");
        return !(hasAuth == null);
    }
}