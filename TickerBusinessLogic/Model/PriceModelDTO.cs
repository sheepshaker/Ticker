using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ticker.Model
{
    public class PriceModelDTO
    {
        public PriceModelDTO(string data)
        {
            //parse data
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException("data");

            var temp = data.Split(':');
            if (temp.Count() != 2)
            {
                throw new ArgumentException("invalid data format");
            }

            Symbol = temp[0];

            decimal price;
            if(decimal.TryParse(temp[1], out price) == false)
            {
                throw new ArgumentException("invalid data format");
            }

            Price = price;
        }

        public string Symbol { get; private set; }
        public decimal Price { get; private set; }
    }
}
