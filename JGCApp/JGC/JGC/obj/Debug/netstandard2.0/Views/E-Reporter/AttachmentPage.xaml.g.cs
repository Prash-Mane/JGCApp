//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("JGC.Views.E_Reporter.AttachmentPage.xaml", "Views/E-Reporter/AttachmentPage.xaml", typeof(global::JGC.Views.E_Reporter.AttachmentPage))]

namespace JGC.Views.E_Reporter {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\E-Reporter\\AttachmentPage.xaml")]
    public partial class AttachmentPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.StackLayout outerStacklayout;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.StackLayout PDFListlayout;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.ListView listView;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::JGC.UserControls.CustomControls.CustomPDF AttachedPDF;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::JGC.UserControls.CustomControls.CustomPicker ListOfPhotoPicker;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::JGC.UserControls.CustomControls.CustomPicker CameraPicker;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::SkiaSharp.Views.Forms.SKCanvasView CameracanvasView;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(AttachmentPage));
            outerStacklayout = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.StackLayout>(this, "outerStacklayout");
            PDFListlayout = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.StackLayout>(this, "PDFListlayout");
            listView = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.ListView>(this, "listView");
            AttachedPDF = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::JGC.UserControls.CustomControls.CustomPDF>(this, "AttachedPDF");
            ListOfPhotoPicker = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::JGC.UserControls.CustomControls.CustomPicker>(this, "ListOfPhotoPicker");
            CameraPicker = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::JGC.UserControls.CustomControls.CustomPicker>(this, "CameraPicker");
            CameracanvasView = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::SkiaSharp.Views.Forms.SKCanvasView>(this, "CameracanvasView");
        }
    }
}
