using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour
{
    public TMP_Dropdown languageDropdown; // TextMeshPro Dropdown UI 요소

    private void Start()
    {
        Locale savedLocale = LocalizationSettings.AvailableLocales.GetLocale(DataManager.instance.gameData.currentLanguage);
        if (savedLocale != null)
        {
            LocalizationSettings.SelectedLocale = savedLocale;
        }
        // 드롭다운 옵션 초기화
        InitializeDropdownOptions();

        // 현재 로케일 인덱스를 드롭다운에 설정
        int currentLocaleIndex = GetLocaleIndex(LocalizationSettings.SelectedLocale);
        languageDropdown.value = currentLocaleIndex;

        // 드롭다운의 값이 변경될 때 언어 변경 함수 호출
        languageDropdown.onValueChanged.AddListener(ChangeLanguage);
    }

    private void InitializeDropdownOptions()
    {
        languageDropdown.ClearOptions(); // 기존 옵션 제거
        var locales = LocalizationSettings.AvailableLocales.Locales;

        foreach (var locale in locales)
        {
            languageDropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
        }
    }

    private void ChangeLanguage(int index)
    {
        Locale newLocale = LocalizationSettings.AvailableLocales.Locales[index];
        LocalizationSettings.SelectedLocale = newLocale;

        DataManager.instance.gameData.currentLanguage = newLocale.Identifier.Code;
    }

    private int GetLocaleIndex(Locale locale)
    {
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code == locale.Identifier.Code)
            {
                return i;
            }
        }
        return 0; // 기본값
    }
}
