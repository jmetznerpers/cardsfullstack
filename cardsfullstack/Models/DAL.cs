using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Dapper;
using System.Net.Http;

//important, some of the http stuff was moved to nuget package
// need Microsoft.AspNet.WebApi.Client to use using System.Net.Http; for readasasync

namespace cardsfullstack.Models
{
    public class DAL
    {
        public static MySqlConnection DB;
        //=========================================
        //
        //High level DB helper functions
        //  This should be the only place where deckresponse and cardresponse classes are used
        //      The rest of the program uses Deck and Card classes
        //
        //=========================================

        // Main Entry into the data side of the app
        // Initialize a deck from api and save it to database
        // Draw 2 cards and save those cards in our database
        // Return those cards

        //wehn bulding an async function wrap return type in task<>
        public static async Task<IEnumerable<Card>> InitializeDeck()
        {
            //step 1: call api for new shuffle deck
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://deckofcardsapi.com");
            var response = await client.GetAsync("/api/deck/new/shuffle/?deck_count=1");
            DeckResponse deckresp = await response.Content.ReadAsAsync<DeckResponse>();

            //step 2: save deck in the database using function
            Deck mydeck = saveDeck(deckresp.deck_id, "user100");

            //step 3: call the api to get 2 cards from the deck (based on deck_id)
            response = await client.GetAsync($"https://deckofcardsapi.com/api/deck/{mydeck.deck_id}/draw/?count=2");
            DeckResponse deckresp2 = await response.Content.ReadAsAsync<DeckResponse>();

            //step 4: save cards to db using function
            foreach(CardResponse cardresp in deckresp2.cards)
            {
                saveCard(mydeck.deck_id, cardresp.image, cardresp.code, "user100");
            }

            //step 5: return list of card instances (not a list of card response instances)
            return getCardsForDeck(mydeck.deck_id);
        }

        //Get More Cards
        //Draw 2 cards  (make deck_id a parameter)
        //Save cards in DB
        //return those cards


        //=========================================
        //
        //LOWER LEVEL DB METHODS
        //The code below has NO api calls and therefore no knowledge of api classes
        //will not use deckresponse or cardresponse ("Seperation of concerns")
        //
        //=========================================

        //create
        //new deck
        public static Deck saveDeck(string deck_id, string username)
        {
            Deck theDeck = new Deck() { deck_id = deck_id, username = username, created_at = DateTime.Now };
            DB.Insert(theDeck);
            return theDeck;
        }

        //read
        //cards in current Deck

        public static IEnumerable<Card> getCardsForDeck(string deck_id) //deck_id the param can have same name
        {
            var p = new
            { deck_id = deck_id };
            IEnumerable<Card> result = DB.Query<Card>("select * from Card where deck_id = @deck_id", p);//replaces p with the query
            return result;
        }

        //latest deck
        public static Deck getLatestDeck()
            
        {
            //to make this work we need using dapper itself
            IEnumerable<Deck> result = DB.Query<Deck>("select * from Deck order by created_at desc limit 1");
            if (result.Count() == 0)
            {
                return null;
            }
            else
            {
                return result.First();
            }
        }

        public static void saveCards(IEnumerable<Card> cards)
        {
            foreach(Card card in cards)
            {
                DB.Insert(card);                
            }
        }

        public static Card saveCard(string deck_id, string image, string cardcode, string username)
        {
            Card thecard = new Card()
            {
                deck_id = deck_id,
                image = image,
                cardcode = cardcode,
                username = username,
                created_at = DateTime.Now
            };
            DB.Insert(thecard);
            return thecard;
        }

        //update
        //save set of cards for the deck



        //delete


    }
}
