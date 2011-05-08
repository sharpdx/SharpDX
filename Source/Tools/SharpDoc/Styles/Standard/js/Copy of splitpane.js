// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
// Original code from Chris Bolson.
// http://blog.cbolson.com/mootools-expandable-columns/
// -------------------------------------------------------------------------------

// splitPaneId: id to the splitPane container
// splitPaneToggleId: id to the splitPane toggle collapse-expand button
// splitPaneResizerId: id to the resizer grip
function SplitPane(splitPaneId, splitPaneToggleId, splitPaneResizerId) {

    //    define column elemnts
    var splitPane = $(splitPaneId);    //    define the column wrapper so as to be able to get the total width via mootools
    var splitPaneToggle = $(splitPaneToggleId);

    var splitPaneChildrens = splitPane.getChildren('div');
    var paneLeft = splitPaneChildrens[0];
    var paneRight = splitPaneChildrens[1];

    //  Define padding (seperator line widths) for column borders as defined in css
    var panePadding = paneRight.getStyle('padding-left').toInt() + paneRight.getStyle('padding-right').toInt();

    // Original size of the left pane
    var paneLeftOriginalWidth = paneLeft.getWidth().toInt();

    //    define snap if required - set to 0 for no snap
    var resizerSnap = 5;

    var splitPaneWidth = splitPane.getWidth() - panePadding;    // total width of wrapper
    var paneLeftMinWidth = 100;                            //    minimum width for columns
    var paneLeftMinWidth_c = paneLeftMinWidth - panePadding;

    var paneRightWidth = splitPaneWidth - paneLeftOriginalWidth;
    paneRight.setStyle("left", paneLeftOriginalWidth);
    paneRight.setStyle("width", paneRightWidth.toInt() - panePadding);

    // Resize right pane when the window is resized
    window.addEvent('resize', function () {
        splitPaneWidth = splitPane.getWidth() - panePadding;
        paneRightWidth = splitPaneWidth.toInt() - paneLeft.getWidth();
        paneRight.setStyle("width", paneRightWidth.toInt() - panePadding);
    });

    // Make the left pane resizable
    paneLeft.makeResizable({
        handle: splitPaneResizerId, // paneLeft.getChildren('.resize'),
        grid: resizerSnap,
        modifiers: { x: 'width', y: false },
        limit: { x: [paneLeftMinWidth, null] },

        onStart: function (el) {
            //    get available width - total width minus right column - minimum col with
            w_avail = splitPaneWidth - paneLeftMinWidth;
        },

        onDrag: function (el) {
            if (el.getWidth() >= w_avail) {
                //    max width reached - stop drag (force max widths)
                el.setStyle("width", w_avail);
            }
            // paneLeft.tween("opacity", Math.Max(paneLeft.getWidth()/100,1);

            //    set center col left position
            paneRight.setStyle("left", paneLeft.getWidth());

            //    define and set center col width (total minus left minus right)
            paneRightWidth = splitPane.getWidth() - panePadding - paneLeft.getWidth();
            paneRight.setStyle("width", paneRightWidth.toInt() - panePadding);
        },

        onComplete: function () {
            //could add final width to form field here
        }
    });
    // paneLeft.removeEvents('mousedown');

    // Handle toggle button collapse-expand events
    splitPaneToggle.addEvent('click', function (event) {
        if (paneLeft.getWidth() < paneLeftMinWidth) {
            splitPaneToggle.set('class', 'collapse');
            paneLeft.morph({
                'width': paneLeftOriginalWidth,
                'opacity': '1'
            });
            paneRight.morph({
                'left': paneLeftOriginalWidth,
                'width': splitPane.getWidth() - paneLeftOriginalWidth - panePadding * 2
            });
        } else {
            splitPaneToggle.set('class', 'expand');
            paneLeft.morph({
                'width': '0',
                'opacity': '0'
            });
            paneRight.morph({
                'left': '0',
                'width': splitPane.getWidth() - panePadding * 2
            });
        }
    });
}  