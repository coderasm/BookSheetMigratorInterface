using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigratorInterface.Controllers
{
    public abstract class BulkAction<T>
    {
        protected IEnumerable<T> items;
        private List<object> results = new List<object>();

        protected BulkAction(IEnumerable<T> items)
        {
            this.items = items;
        } 

        public async Task<List<object>> processItemsAndReturnResults()
        {
            await Task.Run(async () =>
            {
                await processItems();
            });
            return results;
        }

        private async Task processItems()
        {
            foreach (var item in items)
            {
                await processItemAndReturnResult(item);
            }
        }

        private async Task processItemAndReturnResult(T item)
        {
            var result = await process(item);
            results.Add(result);
        }

        protected abstract Task<object> process(T item);
    }
}