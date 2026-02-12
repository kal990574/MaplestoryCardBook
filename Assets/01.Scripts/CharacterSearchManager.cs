using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSearchManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private ApiConfig _apiConfig;

    [Header("검색")]
    [SerializeField] private GameObject _searchPanel;
    [SerializeField] private TMP_InputField _searchInput;
    [SerializeField] private Button _searchButton;

    [Header("결과")]
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private Button _backButton;
    [SerializeField] private RawImage _characterImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _classText;
    [SerializeField] private TMP_Text _worldText;
    [SerializeField] private TMP_Text _guildText;

    [Header("결과 - 추가 기본정보")]
    [SerializeField] private TMP_Text _popularityText;
    [SerializeField] private TMP_Text _createdDateText;

    [Header("결과 - 전투 스탯")]
    [SerializeField] private GameObject _statSection;
    [SerializeField] private TMP_Text _combatPowerText;
    [SerializeField] private TMP_Text _mainStatText;
    [SerializeField] private TMP_Text _hpMpText;
    [SerializeField] private TMP_Text _attackText;
    [SerializeField] private TMP_Text _bossStatText;

    [Header("결과 - 유니온 / 스타포스")]
    [SerializeField] private GameObject _unionStarforceSection;
    [SerializeField] private TMP_Text _unionLevelText;
    [SerializeField] private TMP_Text _unionGradeText;
    [SerializeField] private TMP_Text _starforceText;

    [Header("상태")]
    [SerializeField] private TMP_Text _errorText;

    private MapleStoryApi _api;

    private void Awake()
    {
        _api = new MapleStoryApi(_apiConfig);
        _searchButton.onClick.AddListener(() => Search().Forget());
        _searchInput.onSubmit.AddListener(_ => Search().Forget());
        _backButton.onClick.AddListener(ShowSearchPanel);

        _searchPanel.SetActive(true);
        _resultPanel.SetActive(false);
        _errorText.gameObject.SetActive(false);
        _statSection.SetActive(false);
        _unionStarforceSection.SetActive(false);
    }

    private async UniTaskVoid Search()
    {
        var characterName = _searchInput.text.Trim();
        if (string.IsNullOrEmpty(characterName)) return;

        _searchButton.interactable = false;
        _searchPanel.SetActive(false);
        _errorText.gameObject.SetActive(false);
        _resultPanel.SetActive(false);
        _statSection.SetActive(false);
        _unionStarforceSection.SetActive(false);

        try
        {
            var profile = await _api.GetCharacterProfileAsync(characterName);

            // 기본정보
            if (profile.Basic != null)
            {
                var info = profile.Basic;
                _nameText.text = $"이름: {info.CharacterName}";
                _levelText.text = $"레벨: Lv.{info.CharacterLevel}";
                _classText.text = $"직업: {info.CharacterClass}";
                _worldText.text = $"월드: {info.WorldName}";
                _guildText.text = $"길드: {(string.IsNullOrEmpty(info.CharacterGuildName) ? "-" : info.CharacterGuildName)}";
                _createdDateText.text = $"생성일: {info.CharacterDateCreate ?? "-"}";

                if (!string.IsNullOrEmpty(info.CharacterImage))
                {
                    var texture = await ImageLoader.LoadTextureAsync(info.CharacterImage);
                    if (texture != null)
                        _characterImage.texture = texture;
                }
            }
            else
            {
                _nameText.text = "이름: -";
                _levelText.text = "레벨: -";
                _classText.text = "직업: -";
                _worldText.text = "월드: -";
                _guildText.text = "길드: -";
                _createdDateText.text = "생성일: -";
            }

            // 인기도
            _popularityText.text = profile.Popularity != null
                ? $"인기도: {profile.Popularity.Popularity:N0}"
                : "인기도: -";

            // 전투 스탯
            if (profile.Stat?.FinalStatList != null)
            {
                _combatPowerText.text = $"스탯 공격력: {FormatNumber(GetStatValue(profile.Stat, "최대 스탯공격력"))}";
                _mainStatText.text = $"STR: {GetStatValue(profile.Stat, "STR")}\n" +
                                     $"DEX: {GetStatValue(profile.Stat, "DEX")}\n" +
                                     $"INT: {GetStatValue(profile.Stat, "INT")}\n" +
                                     $"LUK: {GetStatValue(profile.Stat, "LUK")}";
                _hpMpText.text = $"HP: {GetStatValue(profile.Stat, "HP")}  " +
                                 $"MP: {GetStatValue(profile.Stat, "MP")}";
                _attackText.text = $"공격력: {GetStatValue(profile.Stat, "공격력")}  " +
                                   $"마력: {GetStatValue(profile.Stat, "마력")}";
                _bossStatText.text = $"보스뎀: {GetStatValue(profile.Stat, "보스 몬스터 데미지")}%  " +
                                     $"방무: {GetStatValue(profile.Stat, "방어율 무시")}%  " +
                                     $"최종뎀: {GetStatValue(profile.Stat, "최종 데미지")}%\n" +
                                     $"크확: {GetStatValue(profile.Stat, "크리티컬 확률")}%  " +
                                     $"크뎀: {GetStatValue(profile.Stat, "크리티컬 데미지")}%";
                _statSection.SetActive(true);
            }

            // 유니온 / 스타포스
            var hasUnion = profile.Union != null;
            var hasStarforce = profile.Stat?.FinalStatList != null;

            if (hasUnion || hasStarforce)
            {
                _unionLevelText.text = hasUnion
                    ? $"유니온 Lv.{profile.Union.UnionLevel}"
                    : "유니온: -";
                _unionGradeText.text = hasUnion
                    ? profile.Union.UnionGrade
                    : "-";
                _starforceText.text = hasStarforce
                    ? $"총 스타포스: {GetStatValue(profile.Stat, "스타포스")}"
                    : "총 스타포스: -";
                _unionStarforceSection.SetActive(true);
            }

            _resultPanel.SetActive(true);
        }
        catch (System.Exception e)
        {
            _errorText.text = e.Message;
            _errorText.gameObject.SetActive(true);
            _searchPanel.SetActive(true);
        }
        finally
        {
            _searchButton.interactable = true;
        }
    }

    private void ShowSearchPanel()
    {
        _resultPanel.SetActive(false);
        _searchPanel.SetActive(true);
        _errorText.gameObject.SetActive(false);
    }

    private string GetStatValue(CharacterStatResponse stat, string statName)
    {
        var found = stat.FinalStatList.FirstOrDefault(s => s.StatName == statName);
        return found?.StatValue ?? "-";
    }

    private string FormatNumber(string value)
    {
        if (long.TryParse(value, out var number))
            return number.ToString("N0");
        return value;
    }
}