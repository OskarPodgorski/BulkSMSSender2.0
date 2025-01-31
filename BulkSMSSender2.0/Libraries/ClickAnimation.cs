namespace BulkSMSSender2._0;

public class ClickAnimation : Behavior<Button>
{
    protected override void OnAttachedTo(Button button)
    {
        base.OnAttachedTo(button);
        button.Clicked += OnButtonClicked;
    }

    protected override void OnDetachingFrom(Button button)
    {
        base.OnDetachingFrom(button);
        button.Clicked -= OnButtonClicked;
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            await button.ScaleTo(0.88, 40);
            await button.ScaleTo(1, 40);
        }
    }
}
