using OceanNew._keenthemes.libs;

namespace OceanNew._keenthemes;

public interface IKTBootstrapBase
{
    void InitThemeMode();
    
    void InitThemeDirection();

    void InitLayout();
    
    void Init(IKTTheme theme);
}