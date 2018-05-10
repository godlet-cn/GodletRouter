using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GodletRouter
{
    public class routeRegexpOptions
    {
        internal bool strictSlash;
        internal bool useEncodedPath;
    }

    // routeRegexp stores a regexp to match a host or path and information to collect and validate route variables.
    public class RouteRegexp
    {
        // The unmodified template.
        internal string template;

        // The type of match
        internal RegexpType regexpType;

        // Options for matching
        internal routeRegexpOptions options;

        // Expanded regexp.
        internal Regex regexp;

        // Reverse template.
        internal string reverse;

        // Variable names.
        internal List<string> varsN;

        // Variable regexps (validators).
        internal Regex varsR;

        // newRouteRegexp parses a route template and returns a routeRegexp,
        // used to match a host, a path or a query string.
        //
        // It will extract named variables, assemble a regexp to be matched, create
        // a "reverse" template to build URLs and compile regexps to validate variable
        // values used in URL building.
        //
        // Previously we accepted only Python-like identifiers for variable
        // names ([a-zA-Z_][a-zA-Z0-9_]*), but currently the only restriction is that
        // name and pattern can't be empty, and names can't contain a colon.

        public  RouteRegexp(string tpl , RegexpType typ , routeRegexpOptions options )
        {
            // Check if it is well-formed.
            List<int> idxs = braceIndices(tpl);

            // Backup the original.
            string template = tpl;

            // Now let's parse it.
            string defaultPattern = "[^/]+";
            if (typ == RegexpType.regexpTypeQuery)
            {
                defaultPattern = ".*";
            }
            else if (typ == RegexpType.regexpTypeHost)
            {
                defaultPattern = "[^.]+";
            }
            // Only match strict slash if not matching
            if (typ != RegexpType.regexpTypePath)
            {
                options.strictSlash = false;
            }
            // Set a flag for strictSlash.
            bool endSlash = false;
            if (options.strictSlash && tpl.StartsWith("/"))
            {
                tpl = tpl.Substring(0, tpl .Length- 1);
                endSlash = true;
            }

            //varsN := make([]string, len(idxs)/2)
            //varsR := make([]* regexp.Regexp, len(idxs)/2)
            //pattern := bytes.NewBufferString("")
            //pattern.WriteByte('^')
            //reverse := bytes.NewBufferString("")
            //var end int
            //   var err error
            //for i := 0; i<len(idxs); i += 2 {
            //	// Set all values we are interested in.
            //	raw := tpl[end:idxs[i]]
            //       end = idxs[i + 1]

            //       parts := strings.SplitN(tpl[idxs[i] + 1:end - 1], ":", 2)
            //	name := parts[0]
            //       patt := defaultPattern
            //	if len(parts) == 2 {
            //		patt = parts[1]
            //	}
            //	// Name or pattern can't be empty.
            //	if name == "" || patt == "" {
            //		return nil, fmt.Errorf("mux: missing name or pattern in %q",
            //			tpl[idxs[i]:end])
            //	}
            //	// Build the regexp pattern.
            //	fmt.Fprintf(pattern, "%s(?P<%s>%s)", regexp.QuoteMeta(raw), varGroupName(i/2), patt)

            //	// Build the reverse template.
            //	fmt.Fprintf(reverse, "%s%%s", raw)

            //	// Append variable name and compiled pattern.
            //	varsN[i / 2] = name
            //         varsR[i / 2], err = regexp.Compile(fmt.Sprintf("^%s$", patt))
            //	if err != nil {
            //		return nil, err
            //	}
            //}
            //// Add the remaining.
            //raw := tpl[end:]
            //   pattern.WriteString(regexp.QuoteMeta(raw))
            //if options.strictSlash {
            //	pattern.WriteString("[/]?")
            //}
            //if typ == regexpTypeQuery {
            //	// Add the default pattern if the query value is empty
            //	if queryVal := strings.SplitN(template, "=", 2)[1]; queryVal == "" {
            //		pattern.WriteString(defaultPattern)
            //	}
            //}
            //if typ != regexpTypePrefix {
            //	pattern.WriteByte('$')
            //}
            //reverse.WriteString(raw)
            //if endSlash {
            //	reverse.WriteByte('/')
            //}
            //// Compile full regexp.
            //reg, errCompile := regexp.Compile(pattern.String())
            //if errCompile != nil {
            //	return nil, errCompile
            //}

            //// Check for capturing groups which used to work in older versions
            //if reg.NumSubexp() != len(idxs)/2 {

            //       panic(fmt.Sprintf("route %s contains capture groups in its regexp. ", template) +
            //		"Only non-capturing groups are accepted: e.g. (?:pattern) instead of (pattern)")
            //}

            //// Done!
            //return &routeRegexp{
            //	template:   template,
            //	regexpType: typ,
            //	options:    options,
            //	regexp:     reg,
            //	reverse:    reverse.String(),
            //	varsN:      varsN,
            //	varsR:      varsR,
            //}, nil
        }

        //// Match matches the regexp against the URL host or path.
        //public bool Match(HttpListenerRequest req, RouteMatch match)
        //{
        //    if (this.regexpType != RegexpType.regexpTypeHost)
        //    {
        //        if (this.regexpType == RegexpType.regexpTypeQuery)
        //        {
        //            return this.matchQueryString(req);
        //        }
        //        var path = req.Url.AbsolutePath;
        //        if (this.options.useEncodedPath)
        //        {
        //            path = req.Url.EscapedPath();
        //        }
        //        return this.regexp.IsMatch(path);
        //    }
        //    return this.regexp.IsMatch(Route.getHost(req));
        //}


        // getURLQuery returns a single query parameter from a request URL.
        // For a URL with foo=bar&baz=ding, we return only the relevant key
        // value pair for the routeRegexp.
        public string getURLQuery(HttpListenerRequest req)
        {
            //if (this.regexpType != RegexpType.regexpTypeQuery) {
            //               return "";
            //}
            //   req.Url.Query
            //var templateKey  = strings.SplitN(this.template, "=", 2)[0]
            //for key, vals := range req.URL.Query()
            //       {
            //           if key == templateKey && len(vals) > 0 {
            //               return key + "=" + vals[0]

            //       }
            //       }
            return "";
        }

        public bool matchQueryString(HttpListenerRequest req)
        {
            return this.regexp.IsMatch(this.getURLQuery(req));
        }

        // braceIndices returns the first level curly brace indices from a string.
        // It returns an error in case of unbalanced braces.
        public List<int> braceIndices(string s)
        {
            int level = 0, idx = 0;
            List<int> idxs = new List<int>();
            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case '{':
                        level++;
                        if (level == 1)
                        {
                            idx = i;
                        }
                        break;
                    case '}':
                        level--;
                        if (level == 0)
                        {
                            idxs.Add(idx);
                        }
                        else if (level < 0)
                        {
                            throw new Exception("mux: unbalanced braces in " + s);
                        }
                        break;
                }
            }
            if (level != 0)
            {
                throw new Exception("mux: unbalanced braces in " + s);
            }
            return idxs;
        }
    }



    // ----------------------------------------------------------------------------
    // routeRegexpGroup
    // ----------------------------------------------------------------------------

    // routeRegexpGroup groups the route matchers that carry variables.
    public class routeRegexpGroup
    {
        public RouteRegexp host;

        public RouteRegexp path;

        public List<RouteRegexp> queries;

        // setMatch extracts the variables from the URL once a route matches.
        public void setMatch(HttpListenerRequest req, RouteMatch m, Route r)
        {
            //    // Store host variables.
            //    if (this.host != null) {
            //        var host = Route.getHost(req);

            //        var matches = this.host.regexp.FindStringSubmatchIndex(host);

            //        if (matches.Count > 0)
            //        {
            //            Route.extractVars(host, matches, this.host.varsN, m.Vars);
            //        };
            //    }
            //    var path = req.Url.Path;

            //    // Store path variables.
            //    if (this.path != null) {
            //        var matches = this.path.regexp.FindStringSubmatchIndex(path);

            //if (matches.Count > 0)
            //        {
            //            Route.extractVars(path, matches, this.path.varsN, m.Vars);
            //    // Check if we should redirect.
            //    if (this.path.options.strictSlash) {
            //                p1:= strings.HasSuffix(path, "/");

            //                p2:= strings.HasSuffix(v.path.template, "/");

            //        if p1 != p2 {
            //                    u, _:= url.Parse(req.URL.String());

            //            if p1 {
            //                        u.Path = u.Path[:len(u.Path) - 1];

            //            }
            //                    else
            //                    {
            //                        u.Path += "/";

            //            }
            //                    m.Handler = http.RedirectHandler(u.String(), 301);

            //        }
            //            }
            //        }
            //    }
            //    // Store query string variables.
            //    for _, q := range v.queries {
            //        queryURL:= q.getURLQuery(req)

            //matches:= q.regexp.FindStringSubmatchIndex(queryURL)

            //if len(matches) > 0 {
            //            extractVars(queryURL, matches, q.varsN, m.Vars)

            //}
            //}
        }
    }
}
