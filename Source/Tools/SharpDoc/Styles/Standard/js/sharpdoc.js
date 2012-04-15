// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.       
// -------------------------------------------------------------------------------
// SplitPane handling Toc and Conent
// -------------------------------------------------------------------------------

// splitPaneId: id to the splitPane container
// splitPaneToggleId: id to the splitPane toggle collapse-expand button
// splitPaneResizerId: id to the resizer grip

function supports_local_storage() {
  try {
    return 'localStorage' in window && window['localStorage'] !== null && window.localStorage['getItem'] !== null;
  } catch(e){
    return false;
  }
}

/*
  http://webfreak.no/wp/2007/09/05/get-for-mootools-a-way-to-read-get-variables-with-javascript-in-mootools/
Function: $get
	This function provides access to the "get" variable scope + the element anchor

Version: 1.3

Arguments:
	key - string; optional; the parameter key to search for in the url's query string (can also be "#" for the element anchor)
	url - url; optional; the url to check for "key" in, location.href is default

Example:
	>$get("foo","http://example.com/?foo=bar"); //returns "bar"
	>$get("foo"); //returns the value of the "foo" variable if it's present in the current url(location.href)
	>$get("#","http://example.com/#moo"); //returns "moo"
	>$get("#"); //returns the element anchor if any, but from the current url (location.href)
	>$get(,"http://example.com/?foo=bar&bar=foo"); //returns {foo:'bar',bar:'foo'}
	>$get(,"http://example.com/?foo=bar&bar=foo#moo"); //returns {foo:'bar',bar:'foo',hash:'moo'}
	>$get(); //returns same as above, but from the current url (location.href)
	>$get("?"); //returns the query string (without ? and element anchor) from the current url (location.href)

Returns:
	Returns the value of the variable form the provided key, or an object with the current GET variables plus the element anchor (if any)
	Returns "" if the variable is not present in the given query string

Credits:
		Regex from [url=http://www.netlobo.com/url_query_string_javascript.html]http://www.netlobo.com/url_query_string_javascript.html[/url]
		Function by Jens Anders Bakke, webfreak.no
*/
function $get(key,url){
	if(arguments.length < 2) url =location.href;
	if(arguments.length > 0 && key != ""){
		if(key == "#"){
			var regex = new RegExp("[#]([^$]*)");
		} else if(key == "?"){
			var regex = new RegExp("[?]([^#$]*)");
		} else {
			var regex = new RegExp("[?&]"+key+"=([^&#]*)");
		}
		var results = regex.exec(url);
		return (results == null )? "" : results[1];
	} else {
		url = url.split("?");
		var results = {};
			if(url.length > 1){
				url = url[1].split("#");
				if(url.length > 1) results["hash"] = url[1];
				url[0].split("&").each(function(item,index){
					item = item.split("=");
					results[item[0]] = item[1];
				});
			}
		return results;
	}
}


function InstallCodeTabs() {
    var groupTabs = $$('.grouptab');
    groupTabs.each(function (groupTab, groupIndex) {
        var tabs = groupTab.getChildren('.tabs li.tab');
        var content = groupTab.getChildren('.tabcontent');
        tabs.each(function (tab, index) {
            tab.addEvent('click', function () {
                tabs.removeClass('selected');
                content.removeClass('selected');
                tabs[index].addClass('selected');
                content[index].addClass('selected');
            });
        });    
    });  
}

function SplitPane(splitPaneId, splitPaneToggleId, splitPaneResizerId) {

    // Define column elemnts
    var paneLeft = $(splitPaneId); 
    var splitPaneToggle = $(splitPaneToggleId);

    var paneLeftMinWidth = 100;
    var paneLeftOriginalWidth = paneLeft.getWidth();

    // Use localstorage to store toggle state
    if (supports_local_storage()) {
        if (localStorage.getItem('sharpdoc-toggle')) {
            var value = localStorage.getItem('sharpdoc-toggle');
            if (value == 0) {
                splitPaneToggle.set('class', 'expand');
                paneLeft.setStyle('display','none');
            }
            paneLeft.setStyle('width', value);
        }
    }

    //  Snap size for resizer
    var resizerSnap = 5;

    // Make the left cell resizable
    paneLeft.makeResizable({
        handle: splitPaneResizerId,
        grid: resizerSnap,
        modifiers: { x: 'width', y: false },
        limit: { x: [paneLeftMinWidth, null] },
        onDrag: function (el) {
            if (supports_local_storage()) {
                localStorage.setItem('sharpdoc-resize', el.getWidth());
            }
        },
    });

    var topTitle = $$('h1.content-title');
    
    var expandCollapseFunction = function (event) {
        if (paneLeft.getWidth() < paneLeftMinWidth) {
            splitPaneToggle.set('class', 'collapse');
            paneLeft.setStyle('display','block');
            paneLeft.morph({
                'width': paneLeftOriginalWidth,
                'opacity': '1'
            });
            if (supports_local_storage()) {
                localStorage.removeItem('sharpdoc-resize');
            }
        } else {
            splitPaneToggle.set('class', 'expand');
            paneLeft.set('morph', {link: 'chain'}).morph({
                'width': '1',
                'opacity': '0'
            }).morph({'display': 'none'});

            if (supports_local_storage()) {
                localStorage.setItem('sharpdoc-resize', 0);
            }
        }
    };

    // Handle toggle button collapse-expand events
    splitPaneToggle.addEvent('click', expandCollapseFunction);
    if (topTitle.length > 0) {
        topTitle[0].addEvent('click', expandCollapseFunction);
    }
}  