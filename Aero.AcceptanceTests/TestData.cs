using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aero.EF;
using Aero.Model;
using FizzWare.NBuilder;

namespace Aero.AcceptanceTests
{
    internal class TestData
    {
        internal static Contact CreateContact(string addressLine1, string addressLine2, string addressLine3,
            string city, string country, string county, string email, string fax, string name, string phone, string postCode, ContactType contactType)
        {
            return new Contact
            {
                Address1 = addressLine1,
                Address2 = addressLine2,
                Address3 = addressLine3,
                City = city,
                Country = country,
                County = county,
                Email = email,
                Fax = fax,
                Name = name,
                Phone = phone,
                PostCode = postCode,
                Type = contactType
            };
        }

        internal static Vendor CreateVendor(string name, Contact contact)
        {
            return new Vendor
            {
                Name = name,
                Contact = contact
            };
        }

        internal static Customer CreateCustomer(string name, string userName, Contact contact)
        {
            return new Customer
            {
                Name = name,
                UserName = userName,
                Contact = contact
            };
        }

        internal static Part CreatePart(Condition condition, string description, string model, string nsn, string partNumber, decimal price, short qty, string source, DateTime updateDate, Vendor vendor)
        {
            return new Part
            {
                Condition = condition,
                Description = description,
                Model = model,
                NSN = nsn,
                PartNumber = partNumber,
                Price = price,
                Qty = qty,
                Source = source,
                UpdateDate = updateDate,
                Vendor = vendor
            };
        }

        internal static RFQ CreateRFQ(string comment, DateTime? needBy, Part part, Priority priority, short qty, string yourRef, DateTime dateSubmitted, Customer customer, RFQState rfqState)
        {
            return new RFQ
            {
                Comment = comment,
                NeedBy = needBy,
                Part = part,
                Priority = priority,
                Qty = qty,
                YourRef = yourRef,
                Customer = customer,
                RFQState = rfqState,
                DateSubmitted = dateSubmitted
            };
        }

        internal static PO CreatePO(string comment, Customer customer, DateTime deliveryDate, string number, Part part, short qty, decimal unitPrice)
        {
            return new PO
            {
                Comment = comment,
                Customer = customer,
                DeliveryDate = deliveryDate,
                Number = number,
                Part = part,
                Qty = qty,
                UnitPrice = unitPrice
            };
        }

        internal static Priority CreatePriority(string code, string display)
        {
            return new Priority
            {
                Code = code,
                Display = display
            };
        }

        internal static RFQState CreateRfqState(string code, string display)
        {
            return new RFQState
            {
                Code = code,
                Display = display
            };
        }

        internal static void GenerateTestData()
        {
            var contacts = Builder<Contact>.CreateListOfSize(50)
                .TheFirst(25)
                    .With(x => x.Type = ContactType.Businness)
                .TheNext(25)
                    .With(x => x.Type = ContactType.Home)
                .Build();

            var vendors = Builder<Vendor>.CreateListOfSize(10)
                .All().With(x => x.Contact = Pick<Contact>.RandomItemFrom(contacts))
                .Build();

            var parts = Builder<Part>.CreateListOfSize(300)
                .All().With(x => x.Vendor = Pick<Vendor>.RandomItemFrom(vendors))
                .Build();

            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            var routinePriority = TestData.CreatePriority("Routine", "Routine");
            var highPriority = TestData.CreatePriority("High", "High");
            var priorities = new List<Priority>() { routinePriority, aogPriority, highPriority };

            var rfqStateOpen = TestData.CreateRfqState("Open", "Open");
            var rfqStateClosed = TestData.CreateRfqState("Closed", "Closed");
            var rfqStates = new List<RFQState>() { rfqStateOpen, rfqStateClosed };


            using (AeroContainer _context = new AeroContainer())
            {
                foreach(var contact in contacts)
                {
                    _context.Contacts.Add(contact);
                }
                _context.SaveChanges();

                foreach(var vendor in vendors)
                {
                    _context.Vendors.Add(vendor);
                }
                _context.SaveChanges();

                foreach (var part in parts)
                {
                    _context.Parts.Add(part);
                }
                _context.SaveChanges();

                foreach(var priority in priorities)
                {
                    _context.Priorities.Add(priority);
                }
                _context.SaveChanges();

                foreach (var rfqState in rfqStates)
                {
                    _context.RFQStates.Add(rfqState);
                }
                _context.SaveChanges();
            }
        }
    }
}
