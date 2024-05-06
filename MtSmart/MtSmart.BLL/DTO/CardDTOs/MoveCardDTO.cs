﻿namespace MtSmart.BLL.DTO.CardDTOs
{
    public class MoveCardDTO
    {
        public int CardId { get; set; }

        public string CardName { get; set; }
        public string CardRequirement { get; set; }
        public DateOnly CardTerm { get; set; }


        public string UpdaterName { get; set; }
        public string UpdaterImagePath { get; set; }
    }
}
