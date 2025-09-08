using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core
{
    public abstract class MapperBase<TSource, TDestination>
    { 
        public List<TSource> MapFromDestinationToSource(List<TDestination> destinationEntities)
        {
            if (destinationEntities != null)
            {
                List<TSource> sourceList = new List<TSource>();

                foreach (TDestination destinationEntity in destinationEntities)
                {
                    sourceList.Add(MapFromDestinationToSource(destinationEntity));
                } 
                return sourceList;
            } 
            return null;
        } 
       
        public List<TDestination> MapFromSourceToDestination(List<TSource> sourceEntities)
        {
            if (sourceEntities != null)
            {
                List<TDestination> destinationList = new List<TDestination>();

                foreach (TSource sourceEntity in sourceEntities)
                {
                    destinationList.Add(MapFromSourceToDestination(sourceEntity));
                }

                return destinationList;
            }

            return null;
        }
         
        public abstract TSource MapFromDestinationToSource(TDestination destinationEntity);
         
        public abstract TDestination MapFromSourceToDestination(TSource sourceEntity);
    }
}
