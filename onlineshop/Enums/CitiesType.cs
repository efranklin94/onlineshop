using onlineshop.Attributes;
using System.ComponentModel;

namespace onlineshop.Enums;

[EnumEndpoint("/CitiesTypes")]
public enum CitiesType
{
    [Description("شهر بزرگ")]
    [Info("color", "#ffff")]
    [Info("addressCode", 12333123123)]
    Metropolis = 1,
    [Description("پایتخت")]
    Capital = 2,
    [Description("شهرک")]
    [Info("hi","Hello")]
    SmallCity = 3,
    [Info("list", "[1,2,3,4]")]
    [Description("روستا")]
    Village = 4
}
