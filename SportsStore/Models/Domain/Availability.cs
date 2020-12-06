using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models.Domain
{
    public enum Availability
    {
        [Display(Name = "Shop and online")]
        ShopAndOnline,
        [Display(Name = "Shop only")]
        ShopOnly,
        [Display(Name = "Online only")]
        OnlineOnly
    }
}