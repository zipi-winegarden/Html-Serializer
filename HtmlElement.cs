﻿using Practicode2;
using System.Collections.Generic;

public class HtmlElement
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, string> Attributes { get; set; }
    public List<string> Classes { get; set; }
    public string InnerHtml { get; set; }
    public HtmlElement Parent { get; set; }
    public List<HtmlElement> Children { get; set; }

    public HtmlElement()
    {
        Attributes = new Dictionary<string, string>();
        Classes = new List<string>();
        Children = new List<HtmlElement>();
    }

    public IEnumerable<HtmlElement> Descendants()
    {
        var queue = new Queue<HtmlElement>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;

            foreach (var child in current.Children)
            {
                queue.Enqueue(child);
            }
        }
    }

    public IEnumerable<HtmlElement> Ancestors()
    {
        var current = this.Parent;
        while (current != null)
        {
            yield return current;
            current = current.Parent;
        }
    }

    //public HashSet<HtmlElement> FindElementsBySelector(string selector)
    //{
    //    Selector selectorObject = Selector.Parse(selector);
    //    var result = new HashSet<HtmlElement>();
    //    FindElementsBySelectorRecursive(this, selectorObject, result);
    //    return result;
    //}

    //private static void FindElementsBySelectorRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
    //{
    //    if (element == null)
    //        return;
    //    if (MatchesSelector(element, selector))
    //    {
    //        if (selector.Child.Child == null)
    //        {
    //            result.Add(element);
    //            return;
    //        }
    //    }

    //    foreach (var child in element.Descendants())
    //    {
    //        FindElementsBySelectorRecursive(child, selector, result);
    //    }
    //}

    //private static bool MatchesSelector(HtmlElement element, Selector selector)
    //{

    //    if (selector.TagName != element.Name)
    //        return false;
    //    char[] cc = new char[] { '\\', '\"' };
    //    string elementId = null;
    //    if (element.Id != null)
    //        elementId = element.Id.Split(cc).First(c => c != "");
    //    if (elementId != null && !selector.Id.Equals(elementId))
    //        return false;
    //    foreach (var c in selector.Classes)
    //    {
    //        if (element.Classes != null)
    //        {
    //            for (int i = 0; i < element.Classes.Count; i++)
    //            {
    //                element.Classes[i] = element.Classes[i].Split(cc).First(g => g != "");
    //            }
    //            if (!(element.Classes.Contains(c) || element.Classes.Contains(c + "\\\"") || element.Classes.Contains("\"\\" + c)))
    //                return false;
    //        }
    //    }

    //    return true;

    //}
    public HashSet<HtmlElement> FindElement(Selector selector)
    {
        var result = new HashSet<HtmlElement>();
        foreach (var child in this.Descendants())
            child.FindElementRec(selector, result);
        return result;

    }
    public void FindElementRec(Selector selector, HashSet<HtmlElement> result)
    {
        if (!this.IsEqual(selector))
            return;
        if (selector.Child.Child == null)
        {
            result.Add(this);
            return;
        }
        foreach (var child in this.Descendants())
            child.FindElementRec(selector.Child, result);
    }
    public bool IsEqual(Selector selector)
    {
        if (this.Name != selector.TagName)
            return false;
        if (this.Id != selector.Id)
            return false;
        if (this.Classes.Count() != selector.Classes.Count())
            return false;
        int i = 0;
        while (this.Classes.ToList().Count() > i && selector.Classes.ToList().Count() > i)
        {
            var c1 = this.Classes.ToArray()[i];
            var c2 = selector.Classes.ToArray()[i++];
            if (!c1.Equals(c2))
                return false;
        }
        return true;
    }

}

