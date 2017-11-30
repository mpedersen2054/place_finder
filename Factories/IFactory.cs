using PlaceFinder.Models;
using System.Collections.Generic;

namespace PlaceFinder.Factory
{
    public interface IFactory<T> where T : BaseEntity
    {
    }
}