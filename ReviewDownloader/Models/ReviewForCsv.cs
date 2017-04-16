using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReviewDownloader.Models
{
    [DelimitedRecord(","), IgnoreFirst(1)]
    public class ReviewForCsv
    {

        [FieldOrder(1)]
        [FieldQuoted('"', QuoteMode.AlwaysQuoted, MultilineMode.AllowForWrite)]
        public string UserName;

        [FieldOrder(7)]
        [FieldQuoted('"', QuoteMode.AlwaysQuoted, MultilineMode.AllowForWrite)]
        public string UserProfileLink;

        [FieldOrder(4)]
        [FieldQuoted('"', QuoteMode.AlwaysQuoted, MultilineMode.AllowForWrite)]
        public string Title;

        [FieldOrder(5)]
        [FieldQuoted('"', QuoteMode.AlwaysQuoted, MultilineMode.AllowForWrite)]
        public string ReviewComment;

        [FieldOrder(3)]
        public int StarRating;

        [FieldOrder(6)]
        [FieldQuoted('"', QuoteMode.AlwaysQuoted, MultilineMode.AllowForWrite)]
        public string ReviewLink;

        [FieldOrder(2)]
        [FieldConverter(ConverterKind.Date, "dd-MMM-yyyy")]
        public DateTime? Date;
    }
}