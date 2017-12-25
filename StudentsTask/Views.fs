namespace Views

open FsXaml

type App = XAML<"App.xaml">
type MainWin = XAML<"MainWindow.xaml">
type StudentsControl = XAML<"StudentsControl.xaml">
type StudentDialogBase = XAML<"StudentDialog.xaml">

type StudentDialog() =
    inherit StudentDialogBase()

    override this.CloseClick (_sender, _e) = this.Close()