/// <reference path="_references.js" />
"use strict";
//Calls the given comparer function for each item in the array.
//If comparer returns truthy, that item is returned.
Array.prototype.first = function (comparer) {
    for (var i = 0; i < this.length; i++) {
        if (comparer(this[i])) {
            return this[i];
        }
    }
};

//Get first instance of the given item in the given array.
//Search uses strict comparison operator (===) unless we are dealing with strings and caseSensitive is falsey.
//Note: for non-caseSensitive searches, returns the original array item if a match is found.
Array.prototype.getFirst = function (item, caseSensitive) {
    for (var i = 0; i < this.length; i++) {
        if ((!caseSensitive) && Utils.isString(item) && Utils.isString(this[i]) && item.toLowerCase() === this[i].toLowerCase()) {
            return this[i];
        }
        else if (this[i] === item) {
            return this[i];
        }
    }
};

//Checks a Date object if it represents a valid date or not
Date.prototype.isvalid = function () {
    return (!isNaN(this.valueOf()));
};

function Utils() { }

//Returns true if thing is an instance of either a string primitive or String object
Utils.isString = function (thing) {
    return (typeof thing === 'string' || thing instanceof String);
};