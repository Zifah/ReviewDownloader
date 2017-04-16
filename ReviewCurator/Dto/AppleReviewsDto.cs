using System.Collections.Generic;
using System.Xml.Serialization;

namespace ReviewCurator.Dto.AppleReviewDto
{

    [XmlRoot(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
    public class Link
    {
        [XmlAttribute(AttributeName = "rel")]
        public string Rel { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }

    [XmlRoot(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
    public class Author
    {
        [XmlElement(ElementName = "name", Namespace = "http://www.w3.org/2005/Atom")]
        public string Name { get; set; }
        [XmlElement(ElementName = "uri", Namespace = "http://www.w3.org/2005/Atom")]
        public string Uri { get; set; }
    }

    [XmlRoot(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
    public class Id
    {
        [XmlAttribute(AttributeName = "id", Namespace = "http://itunes.apple.com/rss")]
        public string _id { get; set; }
        [XmlAttribute(AttributeName = "bundleId", Namespace = "http://itunes.apple.com/rss")]
        public string BundleId { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "contentType", Namespace = "http://itunes.apple.com/rss")]
    public class ContentType
    {
        [XmlAttribute(AttributeName = "term")]
        public string Term { get; set; }
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }
    }

    [XmlRoot(ElementName = "category", Namespace = "http://www.w3.org/2005/Atom")]
    public class Category
    {
        [XmlAttribute(AttributeName = "id", Namespace = "http://itunes.apple.com/rss")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "term")]
        public string Term { get; set; }
        [XmlAttribute(AttributeName = "scheme")]
        public string Scheme { get; set; }
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }
    }

    [XmlRoot(ElementName = "artist", Namespace = "http://itunes.apple.com/rss")]
    public class Artist
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "price", Namespace = "http://itunes.apple.com/rss")]
    public class Price
    {
        [XmlAttribute(AttributeName = "amount")]
        public string Amount { get; set; }
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "image", Namespace = "http://itunes.apple.com/rss")]
    public class Image
    {
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "releaseDate", Namespace = "http://itunes.apple.com/rss")]
    public class ReleaseDate
    {
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "img", Namespace = "http://www.w3.org/2005/Atom")]
    public class Img
    {
        [XmlAttribute(AttributeName = "border")]
        public string Border { get; set; }
        [XmlAttribute(AttributeName = "alt")]
        public string Alt { get; set; }
        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
    }

    [XmlRoot(ElementName = "a", Namespace = "http://www.w3.org/2005/Atom")]
    public class A
    {
        [XmlElement(ElementName = "img", Namespace = "http://www.w3.org/2005/Atom")]
        public Img Img { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "td", Namespace = "http://www.w3.org/2005/Atom")]
    public class Td
    {
        [XmlElement(ElementName = "a", Namespace = "http://www.w3.org/2005/Atom")]
        public A A { get; set; }
        [XmlAttribute(AttributeName = "align")]
        public string Align { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "valign")]
        public string Valign { get; set; }
        [XmlElement(ElementName = "img", Namespace = "http://www.w3.org/2005/Atom")]
        public Img Img { get; set; }
        [XmlElement(ElementName = "b", Namespace = "http://www.w3.org/2005/Atom")]
        public B B { get; set; }
        [XmlElement(ElementName = "br", Namespace = "http://www.w3.org/2005/Atom")]
        public string Br { get; set; }
        [XmlElement(ElementName = "font", Namespace = "http://www.w3.org/2005/Atom")]
        public Font Font { get; set; }
        [XmlElement(ElementName = "table", Namespace = "http://www.w3.org/2005/Atom")]
        public Table Table { get; set; }
    }

    [XmlRoot(ElementName = "b", Namespace = "http://www.w3.org/2005/Atom")]
    public class B
    {
        [XmlElement(ElementName = "a", Namespace = "http://www.w3.org/2005/Atom")]
        public A A { get; set; }
    }

    [XmlRoot(ElementName = "font", Namespace = "http://www.w3.org/2005/Atom")]
    public class Font
    {
        [XmlElement(ElementName = "br", Namespace = "http://www.w3.org/2005/Atom")]
        public List<string> Br { get; set; }
        [XmlElement(ElementName = "b", Namespace = "http://www.w3.org/2005/Atom")]
        public List<string> B { get; set; }
        [XmlElement(ElementName = "a", Namespace = "http://www.w3.org/2005/Atom")]
        public A A { get; set; }
        [XmlAttribute(AttributeName = "size")]
        public string Size { get; set; }
        [XmlAttribute(AttributeName = "face")]
        public string Face { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "tr", Namespace = "http://www.w3.org/2005/Atom")]
    public class Tr
    {
        [XmlElement(ElementName = "td", Namespace = "http://www.w3.org/2005/Atom")]
        public List<Td> Td { get; set; }
        [XmlAttribute(AttributeName = "valign")]
        public string Valign { get; set; }
        [XmlAttribute(AttributeName = "align")]
        public string Align { get; set; }
    }

    [XmlRoot(ElementName = "table", Namespace = "http://www.w3.org/2005/Atom")]
    public class Table
    {
        [XmlElement(ElementName = "tr", Namespace = "http://www.w3.org/2005/Atom")]
        public List<Tr> Tr { get; set; }
        [XmlAttribute(AttributeName = "border")]
        public string Border { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "cellspacing")]
        public string Cellspacing { get; set; }
        [XmlAttribute(AttributeName = "cellpadding")]
        public string Cellpadding { get; set; }
    }

    [XmlRoot(ElementName = "content", Namespace = "http://www.w3.org/2005/Atom")]
    public class Content
    {
        [XmlElement(ElementName = "table", Namespace = "http://www.w3.org/2005/Atom")]
        public Table Table { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "entry", Namespace = "http://www.w3.org/2005/Atom")]
    public class Entry
    {
        [XmlElement(ElementName = "updated", Namespace = "http://www.w3.org/2005/Atom")]
        public string Updated { get; set; }
        [XmlElement(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
        public Id Id { get; set; }
        [XmlElement(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
        public string Title { get; set; }
        [XmlElement(ElementName = "name", Namespace = "http://itunes.apple.com/rss")]
        public string Name { get; set; }
        [XmlElement(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
        public Link Link { get; set; }
        [XmlElement(ElementName = "contentType", Namespace = "http://itunes.apple.com/rss")]
        public ContentType ContentType { get; set; }
        [XmlElement(ElementName = "category", Namespace = "http://www.w3.org/2005/Atom")]
        public Category Category { get; set; }
        [XmlElement(ElementName = "artist", Namespace = "http://itunes.apple.com/rss")]
        public Artist Artist { get; set; }
        [XmlElement(ElementName = "price", Namespace = "http://itunes.apple.com/rss")]
        public Price Price { get; set; }
        [XmlElement(ElementName = "image", Namespace = "http://itunes.apple.com/rss")]
        public List<Image> Image { get; set; }
        [XmlElement(ElementName = "rights", Namespace = "http://www.w3.org/2005/Atom")]
        public string Rights { get; set; }
        [XmlElement(ElementName = "releaseDate", Namespace = "http://itunes.apple.com/rss")]
        public ReleaseDate ReleaseDate { get; set; }
        [XmlElement(ElementName = "content", Namespace = "http://www.w3.org/2005/Atom")]
        public List<Content> Content { get; set; }
        [XmlElement(ElementName = "voteSum", Namespace = "http://itunes.apple.com/rss")]
        public string VoteSum { get; set; }
        [XmlElement(ElementName = "voteCount", Namespace = "http://itunes.apple.com/rss")]
        public string VoteCount { get; set; }
        [XmlElement(ElementName = "rating", Namespace = "http://itunes.apple.com/rss")]
        public string Rating { get; set; }
        [XmlElement(ElementName = "version", Namespace = "http://itunes.apple.com/rss")]
        public string Version { get; set; }
        [XmlElement(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
        public Author Author { get; set; }
    }

    [XmlRoot(ElementName = "feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class Feed
    {
        [XmlElement(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
        public string Id { get; set; }
        [XmlElement(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
        public string Title { get; set; }
        [XmlElement(ElementName = "updated", Namespace = "http://www.w3.org/2005/Atom")]
        public string Updated { get; set; }
        [XmlElement(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
        public List<Link> Link { get; set; }
        [XmlElement(ElementName = "icon", Namespace = "http://www.w3.org/2005/Atom")]
        public string Icon { get; set; }
        [XmlElement(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
        public Author Author { get; set; }
        [XmlElement(ElementName = "rights", Namespace = "http://www.w3.org/2005/Atom")]
        public string Rights { get; set; }
        [XmlElement(ElementName = "entry", Namespace = "http://www.w3.org/2005/Atom")]
        public List<Entry> Entry { get; set; }
        [XmlAttribute(AttributeName = "im", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Im { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Lang { get; set; }
    }

}
