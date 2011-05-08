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

// splitPaneId: id to the splitPane container
// splitPaneToggleId: id to the splitPane toggle collapse-expand button
// splitPaneResizerId: id to the resizer grip

function supports_local_storage() {
  try {
    return 'localStorage' in window && window['localStorage'] !== null;
  } catch(e){
    return false;
  }
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
        limit: { x: [paneLeftMinWidth, 500] },
        onDrag: function (el) {
            if (supports_local_storage()) {
                localStorage.setItem('sharpdoc-resize', el.getWidth());
            }
        },
    });

    // Handle toggle button collapse-expand events
    splitPaneToggle.addEvent('click', function (event) {
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
    });
}  