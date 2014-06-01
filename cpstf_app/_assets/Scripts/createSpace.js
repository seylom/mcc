function firstshuffle() {
    pid = document.getElementById("lastpid").value;
    if (document.getElementById("item_" + pid)) {
        shiftPosition();
        return;
    }
    else setTimeout("firstshuffle()", 500);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////// POSITION SHIFTER - NEARLY EXACTLY THE SAME AS AUTOLAYOUT /////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var RIGHTPAD = 15;
var TOPPAD = 110;
var LEFTPAD = 30;
var BOTPAD = 15;
var BOXW = 220;
var BOXH = 340;
var ENDPAD = 30;
var FULLSIZE = 690;
var pageW = 0;
var cols = 0;
var maxcols = 6;
var sizeMsg = false;



function shiftPosition() {
    CARDBORDER = 30;

    this.col_ar = [];

    this.init = function() {

        var itemsList = document.getElementById('items').childNodes.length;
        this.pushed_ar = [];
        var found = false;
        pageW = this.getPageW();
        cols = Math.floor((pageW - (LEFTPAD * 2)) / (BOXW + RIGHTPAD));

        for (i = cols; i > 0; i--) {
            if (i <= (cols - 3)) {
                targetSelCol = i;
                break;
            }
        }

        window.onresize = function() {
            shiftPosition();
        }

        ////////////////////// LOOP THROUGH THE LIST. FIND THE NEW SPOT TO OPEN IN
        for (i = 0; i < itemsList; i++) {
            var thisItem = document.getElementById('items').childNodes[i];
            var bottom = thisItem.offsetTop + thisItem.offsetHeight
            var thisCol = ((thisItem.offsetLeft - 30) / (BOXW + BOTPAD)) + 1;
            this.pushed_ar.push(thisItem.id);

        }



        this.col_ar = [];
        if (!cols || cols < 3) cols = 3;
        this.col_ar.push({ x: 0, y: 0 }); //the imprint
        for (var i = 1; i < cols; i++) this.col_ar.push({ x: i, y: 0 });


        this.draw();
    }


    this.getPageW = function() {
        var fw;
        if (self.innerWidth) fw = self.innerWidth + 20;
        else if (document.documentElement && document.documentElement.clientWidth) fw = document.documentElement.clientWidth + 20;
        else if (document.body) fw = document.body.clientWidth + 20;
        return fw;
    }

    this.getHeight = function(id) {
        var d = document.getElementById(id);
        return d.offsetHeight;
    }

    this.draw = function() {
        //this.clear();
        var ar = pushed_ar;
        for (var o in ar) {

            o = ar[o];
            this.col_ar.sort(this.ySort);
            var d = document.getElementById(o);

            //if(d) {
            var W = Math.round(d.offsetWidth / BOXW);


            if (W > 1) this.drawWide(o);
            else {
                var c = this.col_ar[0];

                this.drawItem(o, c.x, c.y);
            }
            //}
        }

    }

    this.drawWide = function(o) {
        var pc_ar = [];
        this.col_ar.sort(this.xSort);
        var d = document.getElementById(o);
        var W = Math.round(d.offsetWidth / BOXW);
        var uc = cols - W;
        /* HIDE STUFF THAT IS TOO BIG FOR THE SCREEN */
        if (uc < 0) {
            return;
        }

        /* RESIZE THE CONTENT TO A SMALLER SIZE */
        if (uc < 0) { uc = 1; W = 2; }

        for (var i = 0; i <= uc; i++) {
            pc_ar.push(this.col_ar[i]);
            var my = 0;
            var mi = null;
            for (var j = 0; j < W; j++) {
                var ty = this.col_ar[i + j].y;
                if (ty >= my) {
                    my = ty;
                    mi = i + j;
                }
            }
            var td = my;
            for (var j = 0; j < W; j++) td += Math.abs(this.col_ar[mi].y - this.col_ar[i + j].y);
            pc_ar[i].d = td;
            pc_ar[i].od = pc_ar[i].y - my;
        }
        pc_ar.sort(this.dSort);
        var x = pc_ar[0].x;
        var y = pc_ar[0].y
        if (pc_ar[0].od < 0) y -= pc_ar[0].od;
        this.col_ar.sort(this.ySort);
        this.drawItem(o, x, y);
    }



    this.xSort = function(a, b) {
        return (a.x - b.x);
    }

    this.ySort = function(a, b) {
        if (a.y == b.y) return (a.x - b.x);
        else return (a.y - b.y);
    }

    this.dSort = function(a, b) {
        if (a.d == b.d) return (a.x - b.x);
        else return (a.d - b.d);
    }

    this.drawItem = function(o, x, y) {
        var d = document.getElementById(o);
        var W = Math.round(d.offsetWidth / BOXW);

        d.style.left = x * (BOXW + RIGHTPAD) + LEFTPAD + "px";
        d.style.top = y + TOPPAD + "px";
        d.style.position = "absolute";
        d.style.visibility = "visible";

        console.log(d.id + ' / ' + d.style.left + ' / ' + d.style.top);

        if (W > 1) {
            this.col_ar.sort(this.xSort);
            for (var i = 0; i < W; i++) this.col_ar[x + i].y = y + this.getHeight(d.id) + BOTPAD;
        } else this.col_ar[0].y = y + this.getHeight(d.id) + BOTPAD;
    }

    this.findSameTop = function(myt, t) {
        if (myt == t) return true;
        else return false;
    }
    this.findProximity = function(myt, myb, b) {
        if (b >= myt && b <= myb) return true;
        else return false;
    }
    this.findClosest = function(p1, p2) {
        if (p1 >= p2) return p1;
        else return p2;
    }
    this.findSameCol = function(t1, t2, c1, c2) {
        if (c1 == c2) {
            if (t1 == t2) return true;
            else return false;
        } else return false
    }


    this.init(pid);

    shiftPagination();
}




function shiftPagination() {
    var pageHeight = 0;
    var scrollPosition = getScrollHeight();
    var itemsList = document.getElementById('items').childNodes.length;

    for (i = 0; i < itemsList; i++) {
        var thisItem = document.getElementById('items').childNodes[i];
        var thisHeight = thisItem.offsetTop + thisItem.offsetHeight;
        if (thisHeight > pageHeight) pageHeight = thisHeight
    }

    var pagination = document.getElementById('gal_pagination');
    pagination.style.top = (pageHeight + 30) + 'px';
    pagination.style.left = '0px';
    pagination.style.position = 'absolute';
    pagination.style.visibility = "visible";
}

function getScrollHeight() {
    var y;
    if (self.pageYOffset) {
        y = self.pageYOffset;
    } else if (document.documentElement && document.documentElement.scrollTop) {
        y = document.documentElement.scrollTop;
    } else if (document.body) {
        y = document.body.scrollTop;
    }
    return parseInt(y) + _getWindowHeight();
}

function _getWindowHeight() {
    if (self.innerWidth) {
        frameWidth = self.innerWidth;
        frameHeight = self.innerHeight;
    } else if (document.documentElement && document.documentElement.clientWidth) {
        frameWidth = document.documentElement.clientWidth;
        frameHeight = document.documentElement.clientHeight;
    } else if (document.body) {
        frameWidth = document.body.clientWidth;
        frameHeight = document.body.clientHeight;
    }
    return parseInt(frameHeight);
}

function arfind(ar, value) {
    for (i = 0; i < ar.length; i++) {
        if (ar[i] == value) {
            spot = i;
            break;
        }
    }
    return spot;
}