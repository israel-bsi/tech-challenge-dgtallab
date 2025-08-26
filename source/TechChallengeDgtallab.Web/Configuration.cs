using MudBlazor;

namespace TechChallengeDgtallab.Web
{
    public class Configuration
    {
        public const string HttpClientName = "default";

        private const string PrincipalColor = "#4E61FF";
        public static readonly MudTheme Theme = new()
        {
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = ["Poppins", "sans-serif"],
                    FontSize = "14",
                }
            },
            PaletteLight = new PaletteLight
            {
                Primary = PrincipalColor,
            }
        };
    }
}