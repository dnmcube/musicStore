using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum ProductTypeEnum
{
    [Display(Name = "Гитара")]
    Guitar = 1,

    [Display(Name = "Пианино")]
    Piano = 2,

    [Display(Name = "Бонго")]
    Bongo = 3,

    [Display(Name = "Саксофон")]
    Sax = 4,

    [Display(Name = "Туба")]
    Tuba = 5,

    [Display(Name = "Кларнет")]
    Clarnet = 6
}