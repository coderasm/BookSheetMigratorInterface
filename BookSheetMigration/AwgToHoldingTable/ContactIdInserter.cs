using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookSheetMigration.AwgToHoldingTable
{
    public abstract class ContactIdInserter
    {
           protected AWGTransactionDTO transaction;

        public bool insertIdIfFound()
        {
            if (entityArgumentsExist())
            {
                var entityArguments = getEntityArguments();
                return insertIdIfAtLeastOneFound(entityArguments);
            }
            return false;
        }

        protected abstract bool entityArgumentsExist();

        protected abstract object[] getEntityArguments();

        private bool insertIdIfAtLeastOneFound(params object[] entityArguments)
        {
            var possibleEntities = findEntities(entityArguments).Result;
            if (foundAtLeastOneEntityIn(possibleEntities))
            {
                return InsertId(possibleEntities);
            }
            return false;
        }

        private bool InsertId(List<DealerContactDTO> contacts)
        {
            if (foundMorethanOneEntityIn(contacts))
            {
                if (ableToSetPossibleEntityByName(contacts))
                    return true;
                if (ableToSetPossibleEntityByTitle(contacts))
                    return true;
            }
            setIdFromFoundEntity(contacts[0]);
            return true;
        }

        private bool ableToSetPossibleEntityByName(List<DealerContactDTO> contacts)
        {
            var nameIntransaction = removePunctuationAndToLower(getNameInTransaction());
            foreach (var contact in contacts)
            {
                if (insertByNameMatch(contact, nameIntransaction))
                    return true;
            }
            return false;
        }

        private bool insertByNameMatch(DealerContactDTO possibleEntity, string nameIntransaction)
        {
            var foundName = getEntityName(possibleEntity);
            var cleanFoundName = removePunctuationAndToLower(foundName);
            if (cleanFoundName.Equals(nameIntransaction))
            {
                setIdFromFoundEntity(possibleEntity);
                return true;
            }
            return false;
        }

        private bool ableToSetPossibleEntityByTitle(List<DealerContactDTO> contacts)
        {
            foreach (var contact in contacts)
            {
                if (insertByTitle(contact))
                    return true;
            }
            return false;
        }

        private bool insertByTitle(DealerContactDTO contact)
        {
            var titleLowerCase = contact.title.ToLower();
            var titleToMatch = "used car manager";
            if (Regex.IsMatch(titleLowerCase, titleToMatch))
            {
                setIdFromFoundEntity(contact);
                return true;
            }
            return false;
        }

        protected abstract string getNameInTransaction();

        protected abstract string getEntityName(DealerContactDTO entity);

        private static string removePunctuationAndToLower(string name)
        {
            name = Regex.Replace(name, "[^\\w\\s]", "");
            name = name.ToLower().Trim();
            return name;
        }

        protected abstract Task<List<DealerContactDTO>> findEntities(params object[] entityArguments);

        private bool foundAtLeastOneEntityIn(List<DealerContactDTO> items)
        {
            return items.Count > 0;
        }
        private bool foundMorethanOneEntityIn(List<DealerContactDTO> possibleEntities)
        {
            return possibleEntities.Count > 1;
        }

        protected abstract void setIdFromFoundEntity(DealerContactDTO entity);
    }
}
