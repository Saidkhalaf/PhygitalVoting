using PM.BL.Domain.elements;
using PM.BL.Domain.flows;
using PM.BL.Domain.projects;
using PM.BL.Domain.questions;
using PM.BL.Domain.subthemes;

namespace PM.DAL.EF;

public static class DataSeeder
{
    public static void Seed(PhygitalDbContext context)
    {
        //projects
        Project p1 = new Project()
        {
            Name = "Lokale verkiezingen",
            ParticipantCount = 100,
            ManagerUser = context.Users.Single(user => user.UserName == "subadmin1@kdg.be"),
            Status = true
        };
        
        Flow f1 = new Flow()
        {
            Name = "Enquête 1",
            Subthemes = new List<Subtheme>(),
            FlowType = FlowType.CIRCULAR,
            Project = p1
        };
        
        Flow f2 = new Flow()
        {
            Name = "Enquête 2",
            Subthemes = new List<Subtheme>(),
            FlowType = FlowType.LINEAR,
            Project = p1
        };
        
        Subtheme subtheme1 = new Subtheme
        {
            Name = "Stadsplanning en Mobiliteit",
            Description = "Dit subthema richt zich op stadsplanning, mobiliteit en ecologie, waarbij de nadruk ligt op investeringen in fietsinfrastructuur, verkeersveiligheid en stedelijke parken.",
            Image = "stadEnMobiliteit.png",
            Elements = new List<Element>(),
            Flow = f1
        };

        Subtheme subtheme2 = new Subtheme
        {
            Name = "Onderwijs en Jeugdparticipatie",
            Description = "Dit subthema behandelt onderwerpen zoals onderwijs, sportactiviteiten en manieren om jongeren te betrekken bij de lokale politiek en besluitvorming.",
            Image = "onderwijsEnJongere.png",
            Elements = new List<Element>(),
            Flow = f1
        };

        Subtheme subtheme3 = new Subtheme
        {
            Name = "Stemgedrag en Betrokkenheid",
            Description = "Dit subthema onderzoekt de redenen achter stemgedrag en manieren om de politieke betrokkenheid en participatie onder jongeren te vergroten.",
            Image = "stemgedragEnBettrokenheid.png",
            Elements = new List<Element>(),
            Flow = f1
        };

        Subtheme subtheme4 = new Subtheme
        {
            Name = "Gemeentelijk Beleid",
            Description = "Dit subthema richt zich op de tevredenheid en betrokkenheid van inwoners bij het gemeentelijke beleid, met aandacht voor huisvesting en vrijetijdsvoorzieningen.",
            Image = "gemeentelijkBeleid.png",
            Elements = new List<Element>(),
            Flow = f1
        };

        Subtheme subtheme5 = new Subtheme
        {
            Name = "Energie en Duurzaamheid",
            Description = "Dit subthema behandelt onderwerpen gerelateerd aan duurzaamheid, energiebeheer en de toekomst van stedelijke voorzieningen, zoals straatverlichting en stadsparken.",
            Image = "duurzaamheidEnEnergie.png",
            Elements = new List<Element>(),
            Flow = f1
        };
        
        Question singleChoice1 = new Question
        {
            Position = 1,
            QuestionText = "Als jij de begroting van je stad of gemeente zou opmaken, waar zou je dan in de komende jaren vooral op inzetten?",
            QuestionType = QuestionType.MULTIPLE,
            ResponseOptions = new List<ResponseOption>()
            {
                new ResponseOption("Natuur en ecologie"),
                new ResponseOption("Vrije tijd, sport, cultuur"),
                new ResponseOption("Huisvesting"),
                new ResponseOption("Onderwijs en kinderopvang"),
                new ResponseOption("Gezondheidszorg en welzijn"),
                new ResponseOption("Verkeersveiligheid en mobiliteit"),
                new ResponseOption("Ondersteunen van lokale handel")
            },
            Subtheme = subtheme1
        };

        Question singleChoice2 = new Question
        {
            Position = 2,
            QuestionText = "\"Er moet meer geïnvesteerd worden in overdekte fietsstallingen aan de bushaltes in onze gemeente.\" Wat vind jij van dit voorstel?",
            QuestionType = QuestionType.SINGLE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Eens"),
                new ResponseOption("Oneens")
            },
            Subtheme = subtheme1
        };

        Question singleChoice3 = new Question
        {
            Position = 3,
            QuestionText = "Waarop wil jij dat de focus wordt gelegd in het nieuwe stadspark?",
            QuestionType = QuestionType.MULTIPLE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Sportinfrastructuur"),
                new ResponseOption("Speeltuin voor kinderen"),
                new ResponseOption("Zitbanken en picknickplaatsen"),
                new ResponseOption("Ruimte voor kleine evenementen"),
                new ResponseOption("Drank- en eetmogelijkheden")
            },
            Subtheme = subtheme1
        };

        Question singleChoice4 = new Question
        {
            Position = 4,
            QuestionText = "Hoe sta jij tegenover deze stelling? \"Mijn stad moet meer investeren in fietspaden\"",
            QuestionType = QuestionType.SINGLE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Akkoord"),
                new ResponseOption("Niet akkoord")
            },
            Subtheme = subtheme1
        };

        Question singleChoice5 = new Question
        {
            Position = 1,
            QuestionText =
                "Wat vind jij van het idee om alle leerlingen van de scholen in onze stad een gratis fiets aan te bieden?",
            QuestionType = QuestionType.SINGLE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Goed idee"),
                new ResponseOption("Slecht idee")
            },
            Subtheme = subtheme2
        };
        
        Question multipleChoice1 = new Question
        {
            Position = 2,
            QuestionText = "Wat zou jou helpen om een keuze te maken tussen de verschillende partijen?",
            QuestionType = QuestionType.MULTIPLE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Meer lessen op school rond de partijprogramma’s"),
                new ResponseOption("Activiteiten in mijn jeugdclub, sportclub… rond de verkiezingen"),
                new ResponseOption("Een bezoek van de partijen aan mijn school, jeugd/sportclub, …"),
                new ResponseOption("Een gesprek met mijn ouders rond de gemeentepolitiek"),
                new ResponseOption("Een debat georganiseerd door een jeugdhuis met de verschillende partijen")
            },
            Subtheme = subtheme2
        };

        Question multipleChoice2 = new Question
        {
            Position = 3,
            QuestionText = "Welke sportactiviteit(en) zou jij graag in je eigen stad of gemeente kunnen beoefenen?",
            QuestionType = QuestionType.MULTIPLE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Tennis"),
                new ResponseOption("Hockey"),
                new ResponseOption("Padel"),
                new ResponseOption("Voetbal"),
                new ResponseOption("Fitness")
            },
            Subtheme = subtheme2
        };

        Question multipleChoice3 = new Question
        {
            Position = 4,
            QuestionText =
                "Aan welke van deze activiteiten zou jij meedoen, om mee te wegen op het beleid van jouw stad of gemeente?",
            QuestionType = QuestionType.MULTIPLE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Deelnemen aan gespreksavonden met schepenen en burgemeester"),
                new ResponseOption("Bijwonen van een gemeenteraad"),
                new ResponseOption("Deelnemen aan een survey uitgestuurd door de stad of gemeente"),
                new ResponseOption(
                    "Een overleg waarbij ik onderwerpen kan aandragen die voor jongeren belangrijk zijn"),
                new ResponseOption("Mee brainstormen over oplossingen voor problemen waar jongeren mee worstelen")
            },
            Subtheme = subtheme2
        };

        Question multipleChoice4 = new Question
        {
            Position = 1, 
            QuestionText = "Jij gaf aan dat je waarschijnlijk niet zal gaan stemmen. Om welke reden(en) zeg je dit?",
            QuestionType = QuestionType.MULTIPLE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Ik heb geen interesse"),
                new ResponseOption("Ik heb geen tijd om te gaan stemmen"),
                new ResponseOption("Ik kan niet naar het stemkantoor gaan"),
                new ResponseOption("Ik denk niet dat mijn stem een verschil zal uitmaken"),
                new ResponseOption("Ik heb geen idee voor wie ik zou moeten stemmen")
            },
            Subtheme = subtheme3
        };

        Question multipleChoice5 = new Question
        {
            Position = 2,
            QuestionText = "Wat zou jou (meer) zin geven om te gaan stemmen?",
            QuestionType = QuestionType.MULTIPLE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Kunnen gaan stemmen op een toffere locatie"),
                new ResponseOption("Online, van thuis uit kunnen stemmen"),
                new ResponseOption("Betere inhoudelijke voorstellen van de politieke partijen"),
                new ResponseOption("Meer aandacht voor jeugd in de programma’s van de partijen"),
                new ResponseOption("Beter weten of mijn stem echt impact heeft")
            },
            Subtheme = subtheme3
        };

        Question scaleQuestion1 = new Question
        {
            Position = 3,
            QuestionText = "Ben jij van plan om te gaan stemmen bij de aankomende lokale verkiezingen?",
            QuestionType = QuestionType.RANGE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Zeker niet"),
                new ResponseOption("Eerder niet"),
                new ResponseOption("Ik weet het nog niet"),
                new ResponseOption("Eerder wel"),
                new ResponseOption("Zeker wel")
            },
            Subtheme = subtheme3
        };

        Question scaleQuestion2 = new Question
        {
            Position = 1,
            QuestionText = "Voel jij je betrokken bij het beleid dat wordt uitgestippeld door je gemeente of stad?",
            QuestionType = QuestionType.RANGE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Ik voel me niet betrokken"),
                new ResponseOption("Ik voel me weinig betrokken"),
                new ResponseOption("Ik weet niet"),
                new ResponseOption("ik voel me een beetje betrokken"),
                new ResponseOption("Ik voel me zeer betrokken")
            },
            Subtheme = subtheme4
        };

        Question scaleQuestion3 = new Question
        {
            Position = 2,
            QuestionText = "In hoeverre ben jij tevreden met het vrijetijdsaanbod in jouw stad of gemeente?",
            QuestionType = QuestionType.RANGE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Zeer ontevreden"),
                new ResponseOption("Ontevreden"),
                new ResponseOption("Niet tevreden en niet ontevreden"),
                new ResponseOption("Tevreden"),
                new ResponseOption("Zeer tevreden")
            },
            Subtheme = subtheme4
        };

        Question scaleQuestion4 = new Question
        {
            Position = 3,
            QuestionText = "In welke mate ben jij het eens met de volgende stelling: \"Mijn stad of gemeente doet voldoende om betaalbare huisvesting mogelijk te maken voor iedereen.\"",
            QuestionType = QuestionType.RANGE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("Helemaal oneens"),
                new ResponseOption("Oneens"),
                new ResponseOption("Noch eens, noch oneens"),
                new ResponseOption("Eens"),
                new ResponseOption("Helemaal eens")
            },
            Subtheme = subtheme4
        };

        Question scaleQuestion5 = new Question
        {
            Position = 1,
            QuestionText = "In welke mate kun jij je vinden in het voorstel om de straatlichten in onze gemeente te doven tussen 23u en 5u?",
            QuestionType = QuestionType.RANGE,
            ResponseOptions = new List<ResponseOption>
            {
                new ResponseOption("ik sta hier helemaal niet achter"),
                new ResponseOption("ik sta hier niet achter"),
                new ResponseOption("Geen mening"),
                new ResponseOption("ik sta hier achter"),
                new ResponseOption("ik sta hier helemaal achter")
            },
            Subtheme = subtheme5
        };
        
        Question openQuestion1 = new Question
        {
            Position = 2,
            QuestionText = "Je bent schepen van onderwijs voor een dag: waar zet je dan vooral op in?",
            QuestionType = QuestionType.OPEN,
            Subtheme = subtheme5
        };

        Question openQuestion2 = new Question
        {
            Position = 3,
            QuestionText = "Als je één ding mag wensen voor het nieuwe stadspark, wat zou jouw droomstadspark dan zeker bevatten?",
            QuestionType = QuestionType.OPEN,
            Subtheme = subtheme5
        };
        
        //projects
        context.Projects.Add(p1);
        
        context.Flows.Add(f1);
        context.Flows.Add(f2);
        
        context.Subthemes.Add(subtheme1);
        context.Subthemes.Add(subtheme2);
        context.Subthemes.Add(subtheme3);
        context.Subthemes.Add(subtheme4);
        context.Subthemes.Add(subtheme5);
        
        context.Questions.Add(singleChoice1);
        context.Questions.Add(singleChoice2);
        context.Questions.Add(singleChoice3);
        context.Questions.Add(singleChoice4);
        context.Questions.Add(singleChoice5);
        context.Questions.Add(multipleChoice1);
        context.Questions.Add(multipleChoice2);
        context.Questions.Add(multipleChoice3);
        context.Questions.Add(multipleChoice4);
        context.Questions.Add(multipleChoice5);
        context.Questions.Add(scaleQuestion1);
        context.Questions.Add(scaleQuestion2);
        context.Questions.Add(scaleQuestion3);
        context.Questions.Add(scaleQuestion4);
        context.Questions.Add(scaleQuestion5);
        context.Questions.Add(openQuestion1);
        context.Questions.Add(openQuestion2);
        
        context.SaveChanges();
        
        context.ChangeTracker.Clear();
    }
}