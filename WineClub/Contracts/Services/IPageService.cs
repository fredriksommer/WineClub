using System;

namespace WineClub.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);
    }
}
