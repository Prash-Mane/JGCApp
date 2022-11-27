
namespace JGC.Common.Enumerators
{
    public class Enumerators
    {
        public enum TouchManipulationMode
        {
            None,
            PanOnly,
            IsotropicScale,     // includes panning
            AnisotropicScale,   // includes panning
            ScaleRotate,        // implies isotropic scaling
            ScaleDualRotate     // adds one-finger rotation    
        }
    }
    public enum ApplicationActivity
    {
        LoginPage = 1,
        ProjectListPage=2,
        EReportSelectionPage=3,
        DashboardPage=4,       
        DownloadPage=5,
        DWR_EReporterPage = 6,
        RIR_EReporterPage = 7,
        MRR_EReporterPage = 8,
        CMR_EReporterPage= 9,
        UploadPage=10,
        SettingPage = 11,
        ShowWrapTextPopup=12,
        ModulesPage=13,
        PDFviever = 14,

        //test package
        ETestPackageList = 15,
        PackageDetailPage = 16,
        PunchOverviewPage = 17,
        PunchViewPage = 18,
        TestRecordPage = 19,
        ControlLogPage = 20,
        PandidPage=21,
        DrainRecordPage = 22,
        PreTestRecordPage = 34,
        PostTestRecordPage = 35,

        //Work_Pack
        IWPSelectionPage = 23,
        IWPPdfPage = 24,
        DrawingsPage = 25,
        IWPControlLogPage = 26,
        CWPTagStatusPage = 27,
        ManPowerResourcePage = 28,
        SuccessorPage = 29,
        PredecessorPage = 30,
        PunchControlPage = 31,
        //presetPunch
        AddPunchControlPage = 52,
        EditPunchControlPage = 53,

        MasterNavigationPage = 32,

        //new DWR PAge
        AttachmentPage = 36,
        DWRControlLogPage = 37,
        DWRDrawingPage = 38,
        JointDetailsPage = 39,
        InspectJointPage = 40,

        //Completions
        CompletionProjectList = 41,
        CompletionsDashboard = 42,
        CompletionPunchList = 43,
        CompletionDrawingPage = 44,
        CompletionTestPack = 45,
        CreateNewPunchPage = 46,
        DocumentViewerPopup = 47,
        TagRegisterPage = 48,
        SyncPage = 49,
        PunchListPage = 50,
        HandOverpage = 51,

    }


}
