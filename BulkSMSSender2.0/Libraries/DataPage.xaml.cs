namespace BulkSMSSender2._0;

public partial class DataPage : ContentPage
{
	public DataPage()
	{
		InitializeComponent();
	}

	private void ClearEditorField(object sender, EventArgs e)
	{
		textEditor.Text = string.Empty;
	}
}