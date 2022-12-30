using ChristmasPastryShop.Core.Contracts;
using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories.Contracts;
using ChristmasPastryShop.Utilities.Messages;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using System.Drawing;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Models.Booths;
using ChristmasPastryShop.Models.Cocktails;
using ChristmasPastryShop.Models.Delicacies;

namespace ChristmasPastryShop.Core
{
    public class Controller : IController
    {
        private IRepository<IBooth> booths;

        public Controller()
        {
            this.booths = new BoothRepository();
        }

        public string AddBooth(int capacity)
        {
            Booth booth = new Booth(booths.Models.Count + 1, capacity);
            booths.AddModel(booth);
            return string.Format(Utilities.Messages.OutputMessages.NewBoothAdded, booths.Models.Count, capacity);
        }

        public string AddCocktail(int boothId, string cocktailTypeName, string cocktailName, string size)
        {
            if (cocktailTypeName != "Hibernation" && cocktailTypeName != "MulledWine")
                return string.Format(Utilities.Messages.OutputMessages.InvalidCocktailType, cocktailTypeName);

            if (size != "Small" && size != "Middle" && size != "Large")
                return string.Format(Utilities.Messages.OutputMessages.InvalidCocktailSize, size);

            ICocktail cocktail = null;

            if (cocktailTypeName == "Hibernation")
                cocktail = new Hibernation(cocktailName, size);
            else
                cocktail = new MulledWine(cocktailName, size);

            var selectedBooth = booths.Models.Where(x => x.BoothId == boothId).First();

            if (selectedBooth.CocktailMenu.Models.Any(x => x.Name == cocktailName && x.Size == size))
                return string.Format(Utilities.Messages.OutputMessages.CocktailAlreadyAdded, size, cocktailName);
            else
            {
                selectedBooth.CocktailMenu.AddModel(cocktail);
                return string.Format(Utilities.Messages.OutputMessages.NewCocktailAdded, size, cocktailName, cocktailTypeName);
            }
        }

        public string AddDelicacy(int boothId, string delicacyTypeName, string delicacyName)
        {
            if (delicacyTypeName != "Gingerbread" && delicacyTypeName != "Stolen")
                return string.Format(Utilities.Messages.OutputMessages.InvalidDelicacyType, delicacyTypeName);

            IDelicacy delicacy = null;

            if (delicacyTypeName == "Gingerbread")
                delicacy = new Gingerbread(delicacyName);
            else
                delicacy = new Stolen(delicacyName);

            var selectedBooth = booths.Models.Where(x => x.BoothId == boothId).First();

            if (selectedBooth.DelicacyMenu.Models.Any(x => x.Name == delicacyName))
                return string.Format(Utilities.Messages.OutputMessages.DelicacyAlreadyAdded, delicacyName);
            else
            {
                selectedBooth.DelicacyMenu.AddModel(delicacy);
                return string.Format(Utilities.Messages.OutputMessages.NewDelicacyAdded, delicacyTypeName, delicacyName);
            }
        }

        public string BoothReport(int boothId) => booths.Models.First(x => x.BoothId == boothId).ToString();

        public string LeaveBooth(int boothId)
        {
            var selectedBooth = booths.Models.First(x => x.BoothId == boothId);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Bill {selectedBooth.CurrentBill:f2} lv");

            selectedBooth.Charge();
            selectedBooth.ChangeStatus();
            
            sb.AppendLine($"Booth {boothId} is now available!");

            return sb.ToString().TrimEnd();
        }

        public string ReserveBooth(int countOfPeople)
        {
            var selectedBooth = booths.Models.OrderBy(x => x.Capacity).ThenByDescending(x => x.BoothId).Where(y => !y.IsReserved && y.Capacity >= countOfPeople).FirstOrDefault();

            if (selectedBooth == default)
                return string.Format(Utilities.Messages.OutputMessages.NoAvailableBooth, countOfPeople);
            
            selectedBooth.ChangeStatus();
            return string.Format(Utilities.Messages.OutputMessages.BoothReservedSuccessfully, selectedBooth.BoothId, countOfPeople);            
        }

        public string TryOrder(int boothId, string order)
        {
            string[] commands = order.Split("/");
            string itemTypeName = commands[0];
            string itemName = commands[1];
            int countOrderedPieces = int.Parse(commands[2]);
            string sizeOfCocktail = null;

            ICocktail selectedCocktail = null;
            IDelicacy selectedDelicacy = null;

            if (itemTypeName != "MulledWine" && itemTypeName != "Hibernation" && itemTypeName != "Gingerbread" && itemTypeName != "Stolen")
                return string.Format(Utilities.Messages.OutputMessages.NotRecognizedType, itemTypeName);

            if (commands.Length == 4)
            {
                sizeOfCocktail = commands[3];

                if (itemTypeName == "MulledWine")
                    selectedCocktail = new MulledWine(itemName, sizeOfCocktail);
                else
                    selectedCocktail = new Hibernation(itemName, sizeOfCocktail);
            }
            else
            {
                if (itemTypeName == "Gingerbread")
                    selectedDelicacy = new Gingerbread(itemName);
                else
                    selectedDelicacy = new Stolen(itemName);
            }

            var selectedBooth = booths.Models.First(x => x.BoothId == boothId);

            if (selectedDelicacy != null)
            {
                if (selectedBooth.DelicacyMenu.Models.Any(x => x.Name == itemName))
                {
                    if (selectedBooth.DelicacyMenu.Models.Any(x => x.Name == itemName && x.GetType().Name == itemTypeName))
                    {
                        selectedBooth.UpdateCurrentBill(selectedDelicacy.Price * countOrderedPieces);
                        return string.Format(Utilities.Messages.OutputMessages.SuccessfullyOrdered, boothId, countOrderedPieces, itemName);
                    }
                    
                    return string.Format(Utilities.Messages.OutputMessages.DelicacyStillNotAdded, itemTypeName, itemName);
                    
                }
                else
                    return string.Format(Utilities.Messages.OutputMessages.DelicacyStillNotAdded, itemTypeName, itemName);
            }
            else
            {
                if (selectedBooth.CocktailMenu.Models.Any(x => x.Name == itemName))
                {
                    if (selectedBooth.CocktailMenu.Models.Any(x => x.Size == sizeOfCocktail))
                    {
                        if (selectedBooth.CocktailMenu.Models.Any(x => x.GetType().Name == itemTypeName))
                            selectedBooth.UpdateCurrentBill(selectedCocktail.Price * countOrderedPieces);

                        return string.Format(Utilities.Messages.OutputMessages.SuccessfullyOrdered, boothId, countOrderedPieces, itemName);
                    }
                    else
                        return string.Format(Utilities.Messages.OutputMessages.CocktailStillNotAdded, sizeOfCocktail, itemName);
                }
                else
                    return string.Format(Utilities.Messages.OutputMessages.CocktailStillNotAdded, itemTypeName, itemName);
            }
        }
    }
}