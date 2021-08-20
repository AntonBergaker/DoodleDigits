﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Utilities {
    internal static class EmojiList {
        public static readonly string[] AllEmoji = new[] 
            { "😀", "😃", "😄", "😁", "😆", "😅", "🤣", "😂", "🙂", "🙃", "😉", "😊", "😇", "🥰", "😍", "🤩", "😘", "😗", "☺️", "☺", "😚", "😙", "🥲", "😋", "😛", "😜", "🤪", "😝", "🤑", "🤗", "🤭", "🤫", "🤔", "🤐", "🤨", "😐", "😑", "😶", "😶‍🌫️", "😶‍🌫", "😏", "😒", "🙄", "😬", "😮‍💨", "🤥", "😌", "😔", "😪", "🤤", "😴", "😷", "🤒", "🤕", "🤢", "🤮", "🤧", "🥵", "🥶", "🥴", "😵", "😵‍💫", "🤯", "🤠", "🥳", "🥸", "😎", "🤓", "🧐", "😕", "😟", "🙁", "☹️", "☹", "😮", "😯", "😲", "😳", "🥺", "😦", "😧", "😨", "😰", "😥", "😢", "😭", "😱", "😖", "😣", "😞", "😓", "😩", "😫", "🥱", "😤", "😡", "😠", "🤬", "😈", "👿", "💀", "☠️", "☠", "💩", "🤡", "👹", "👺", "👻", "👽", "👾", "🤖", "😺", "😸", "😹", "😻", "😼", "😽", "🙀", "😿", "😾", "🙈", "🙉", "🙊", "💋", "💌", "💘", "💝", "💖", "💗", "💓", "💞", "💕", "💟", "❣️", "❣", "💔", "❤️‍🔥", "❤‍🔥", "❤️‍🩹", "❤‍🩹", "❤️", "❤", "🧡", "💛", "💚", "💙", "💜", "🤎", "🖤", "🤍", "💯", "💢", "💥", "💫", "💦", "💨", "🕳️", "🕳", "💣", "💬", "👁️‍🗨️", "👁‍🗨️", "👁️‍🗨", "👁‍🗨", "🗨️", "🗨", "🗯️", "🗯", "💭", "💤", "👋", "👋🏻", "👋🏼", "👋🏽", "👋🏾", "👋🏿", "🤚", "🤚🏻", "🤚🏼", "🤚🏽", "🤚🏾", "🤚🏿", "🖐️", "🖐", "🖐🏻", "🖐🏼", "🖐🏽", "🖐🏾", "🖐🏿", "✋", "✋🏻", "✋🏼", "✋🏽", "✋🏾", "✋🏿", "🖖", "🖖🏻", "🖖🏼", "🖖🏽", "🖖🏾", "🖖🏿", "👌", "👌🏻", "👌🏼", "👌🏽", "👌🏾", "👌🏿", "🤌", "🤌🏻", "🤌🏼", "🤌🏽", "🤌🏾", "🤌🏿", "🤏", "🤏🏻", "🤏🏼", "🤏🏽", "🤏🏾", "🤏🏿", "✌️", "✌", "✌🏻", "✌🏼", "✌🏽", "✌🏾", "✌🏿", "🤞", "🤞🏻", "🤞🏼", "🤞🏽", "🤞🏾", "🤞🏿", "🤟", "🤟🏻", "🤟🏼", "🤟🏽", "🤟🏾", "🤟🏿", "🤘", "🤘🏻", "🤘🏼", "🤘🏽", "🤘🏾", "🤘🏿", "🤙", "🤙🏻", "🤙🏼", "🤙🏽", "🤙🏾", "🤙🏿", "👈", "👈🏻", "👈🏼", "👈🏽", "👈🏾", "👈🏿", "👉", "👉🏻", "👉🏼", "👉🏽", "👉🏾", "👉🏿", "👆", "👆🏻", "👆🏼", "👆🏽", "👆🏾", "👆🏿", "🖕", "🖕🏻", "🖕🏼", "🖕🏽", "🖕🏾", "🖕🏿", "👇", "👇🏻", "👇🏼", "👇🏽", "👇🏾", "👇🏿", "☝️", "☝", "☝🏻", "☝🏼", "☝🏽", "☝🏾", "☝🏿", "👍", "👍🏻", "👍🏼", "👍🏽", "👍🏾", "👍🏿", "👎", "👎🏻", "👎🏼", "👎🏽", "👎🏾", "👎🏿", "✊", "✊🏻", "✊🏼", "✊🏽", "✊🏾", "✊🏿", "👊", "👊🏻", "👊🏼", "👊🏽", "👊🏾", "👊🏿", "🤛", "🤛🏻", "🤛🏼", "🤛🏽", "🤛🏾", "🤛🏿", "🤜", "🤜🏻", "🤜🏼", "🤜🏽", "🤜🏾", "🤜🏿", "👏", "👏🏻", "👏🏼", "👏🏽", "👏🏾", "👏🏿", "🙌", "🙌🏻", "🙌🏼", "🙌🏽", "🙌🏾", "🙌🏿", "👐", "👐🏻", "👐🏼", "👐🏽", "👐🏾", "👐🏿", "🤲", "🤲🏻", "🤲🏼", "🤲🏽", "🤲🏾", "🤲🏿", "🤝", "🙏", "🙏🏻", "🙏🏼", "🙏🏽", "🙏🏾", "🙏🏿", "✍️", "✍", "✍🏻", "✍🏼", "✍🏽", "✍🏾", "✍🏿", "💅", "💅🏻", "💅🏼", "💅🏽", "💅🏾", "💅🏿", "🤳", "🤳🏻", "🤳🏼", "🤳🏽", "🤳🏾", "🤳🏿", "💪", "💪🏻", "💪🏼", "💪🏽", "💪🏾", "💪🏿", "🦾", "🦿", "🦵", "🦵🏻", "🦵🏼", "🦵🏽", "🦵🏾", "🦵🏿", "🦶", "🦶🏻", "🦶🏼", "🦶🏽", "🦶🏾", "🦶🏿", "👂", "👂🏻", "👂🏼", "👂🏽", "👂🏾", "👂🏿", "🦻", "🦻🏻", "🦻🏼", "🦻🏽", "🦻🏾", "🦻🏿", "👃", "👃🏻", "👃🏼", "👃🏽", "👃🏾", "👃🏿", "🧠", "🫀", "🫁", "🦷", "🦴", "👀", "👁️", "👁", "👅", "👄", "👶", "👶🏻", "👶🏼", "👶🏽", "👶🏾", "👶🏿", "🧒", "🧒🏻", "🧒🏼", "🧒🏽", "🧒🏾", "🧒🏿", "👦", "👦🏻", "👦🏼", "👦🏽", "👦🏾", "👦🏿", "👧", "👧🏻", "👧🏼", "👧🏽", "👧🏾", "👧🏿", "🧑", "🧑🏻", "🧑🏼", "🧑🏽", "🧑🏾", "🧑🏿", "👱", "👱🏻", "👱🏼", "👱🏽", "👱🏾", "👱🏿", "👨", "👨🏻", "👨🏼", "👨🏽", "👨🏾", "👨🏿", "🧔", "🧔🏻", "🧔🏼", "🧔🏽", "🧔🏾", "🧔🏿", "🧔‍♂️", "🧔‍♂", "🧔🏻‍♂️", "🧔🏻‍♂", "🧔🏼‍♂️", "🧔🏼‍♂", "🧔🏽‍♂️", "🧔🏽‍♂", "🧔🏾‍♂️", "🧔🏾‍♂", "🧔🏿‍♂️", "🧔🏿‍♂", "🧔‍♀️", "🧔‍♀", "🧔🏻‍♀️", "🧔🏻‍♀", "🧔🏼‍♀️", "🧔🏼‍♀", "🧔🏽‍♀️", "🧔🏽‍♀", "🧔🏾‍♀️", "🧔🏾‍♀", "🧔🏿‍♀️", "🧔🏿‍♀", "👨‍🦰", "👨🏻‍🦰", "👨🏼‍🦰", "👨🏽‍🦰", "👨🏾‍🦰", "👨🏿‍🦰", "👨‍🦱", "👨🏻‍🦱", "👨🏼‍🦱", "👨🏽‍🦱", "👨🏾‍🦱", "👨🏿‍🦱", "👨‍🦳", "👨🏻‍🦳", "👨🏼‍🦳", "👨🏽‍🦳", "👨🏾‍🦳", "👨🏿‍🦳", "👨‍🦲", "👨🏻‍🦲", "👨🏼‍🦲", "👨🏽‍🦲", "👨🏾‍🦲", "👨🏿‍🦲", "👩", "👩🏻", "👩🏼", "👩🏽", "👩🏾", "👩🏿", "👩‍🦰", "👩🏻‍🦰", "👩🏼‍🦰", "👩🏽‍🦰", "👩🏾‍🦰", "👩🏿‍🦰", "🧑‍🦰", "🧑🏻‍🦰", "🧑🏼‍🦰", "🧑🏽‍🦰", "🧑🏾‍🦰", "🧑🏿‍🦰", "👩‍🦱", "👩🏻‍🦱", "👩🏼‍🦱", "👩🏽‍🦱", "👩🏾‍🦱", "👩🏿‍🦱", "🧑‍🦱", "🧑🏻‍🦱", "🧑🏼‍🦱", "🧑🏽‍🦱", "🧑🏾‍🦱", "🧑🏿‍🦱", "👩‍🦳", "👩🏻‍🦳", "👩🏼‍🦳", "👩🏽‍🦳", "👩🏾‍🦳", "👩🏿‍🦳", "🧑‍🦳", "🧑🏻‍🦳", "🧑🏼‍🦳", "🧑🏽‍🦳", "🧑🏾‍🦳", "🧑🏿‍🦳", "👩‍🦲", "👩🏻‍🦲", "👩🏼‍🦲", "👩🏽‍🦲", "👩🏾‍🦲", "👩🏿‍🦲", "🧑‍🦲", "🧑🏻‍🦲", "🧑🏼‍🦲", "🧑🏽‍🦲", "🧑🏾‍🦲", "🧑🏿‍🦲", "👱‍♀️", "👱‍♀", "👱🏻‍♀️", "👱🏻‍♀", "👱🏼‍♀️", "👱🏼‍♀", "👱🏽‍♀️", "👱🏽‍♀", "👱🏾‍♀️", "👱🏾‍♀", "👱🏿‍♀️", "👱🏿‍♀", "👱‍♂️", "👱‍♂", "👱🏻‍♂️", "👱🏻‍♂", "👱🏼‍♂️", "👱🏼‍♂", "👱🏽‍♂️", "👱🏽‍♂", "👱🏾‍♂️", "👱🏾‍♂", "👱🏿‍♂️", "👱🏿‍♂", "🧓", "🧓🏻", "🧓🏼", "🧓🏽", "🧓🏾", "🧓🏿", "👴", "👴🏻", "👴🏼", "👴🏽", "👴🏾", "👴🏿", "👵", "👵🏻", "👵🏼", "👵🏽", "👵🏾", "👵🏿", "🙍", "🙍🏻", "🙍🏼", "🙍🏽", "🙍🏾", "🙍🏿", "🙍‍♂️", "🙍‍♂", "🙍🏻‍♂️", "🙍🏻‍♂", "🙍🏼‍♂️", "🙍🏼‍♂", "🙍🏽‍♂️", "🙍🏽‍♂", "🙍🏾‍♂️", "🙍🏾‍♂", "🙍🏿‍♂️", "🙍🏿‍♂", "🙍‍♀️", "🙍‍♀", "🙍🏻‍♀️", "🙍🏻‍♀", "🙍🏼‍♀️", "🙍🏼‍♀", "🙍🏽‍♀️", "🙍🏽‍♀", "🙍🏾‍♀️", "🙍🏾‍♀", "🙍🏿‍♀️", "🙍🏿‍♀", "🙎", "🙎🏻", "🙎🏼", "🙎🏽", "🙎🏾", "🙎🏿", "🙎‍♂️", "🙎‍♂", "🙎🏻‍♂️", "🙎🏻‍♂", "🙎🏼‍♂️", "🙎🏼‍♂", "🙎🏽‍♂️", "🙎🏽‍♂", "🙎🏾‍♂️", "🙎🏾‍♂", "🙎🏿‍♂️", "🙎🏿‍♂", "🙎‍♀️", "🙎‍♀", "🙎🏻‍♀️", "🙎🏻‍♀", "🙎🏼‍♀️", "🙎🏼‍♀", "🙎🏽‍♀️", "🙎🏽‍♀", "🙎🏾‍♀️", "🙎🏾‍♀", "🙎🏿‍♀️", "🙎🏿‍♀", "🙅", "🙅🏻", "🙅🏼", "🙅🏽", "🙅🏾", "🙅🏿", "🙅‍♂️", "🙅‍♂", "🙅🏻‍♂️", "🙅🏻‍♂", "🙅🏼‍♂️", "🙅🏼‍♂", "🙅🏽‍♂️", "🙅🏽‍♂", "🙅🏾‍♂️", "🙅🏾‍♂", "🙅🏿‍♂️", "🙅🏿‍♂", "🙅‍♀️", "🙅‍♀", "🙅🏻‍♀️", "🙅🏻‍♀", "🙅🏼‍♀️", "🙅🏼‍♀", "🙅🏽‍♀️", "🙅🏽‍♀", "🙅🏾‍♀️", "🙅🏾‍♀", "🙅🏿‍♀️", "🙅🏿‍♀", "🙆", "🙆🏻", "🙆🏼", "🙆🏽", "🙆🏾", "🙆🏿", "🙆‍♂️", "🙆‍♂", "🙆🏻‍♂️", "🙆🏻‍♂", "🙆🏼‍♂️", "🙆🏼‍♂", "🙆🏽‍♂️", "🙆🏽‍♂", "🙆🏾‍♂️", "🙆🏾‍♂", "🙆🏿‍♂️", "🙆🏿‍♂", "🙆‍♀️", "🙆‍♀", "🙆🏻‍♀️", "🙆🏻‍♀", "🙆🏼‍♀️", "🙆🏼‍♀", "🙆🏽‍♀️", "🙆🏽‍♀", "🙆🏾‍♀️", "🙆🏾‍♀", "🙆🏿‍♀️", "🙆🏿‍♀", "💁", "💁🏻", "💁🏼", "💁🏽", "💁🏾", "💁🏿", "💁‍♂️", "💁‍♂", "💁🏻‍♂️", "💁🏻‍♂", "💁🏼‍♂️", "💁🏼‍♂", "💁🏽‍♂️", "💁🏽‍♂", "💁🏾‍♂️", "💁🏾‍♂", "💁🏿‍♂️", "💁🏿‍♂", "💁‍♀️", "💁‍♀", "💁🏻‍♀️", "💁🏻‍♀", "💁🏼‍♀️", "💁🏼‍♀", "💁🏽‍♀️", "💁🏽‍♀", "💁🏾‍♀️", "💁🏾‍♀", "💁🏿‍♀️", "💁🏿‍♀", "🙋", "🙋🏻", "🙋🏼", "🙋🏽", "🙋🏾", "🙋🏿", "🙋‍♂️", "🙋‍♂", "🙋🏻‍♂️", "🙋🏻‍♂", "🙋🏼‍♂️", "🙋🏼‍♂", "🙋🏽‍♂️", "🙋🏽‍♂", "🙋🏾‍♂️", "🙋🏾‍♂", "🙋🏿‍♂️", "🙋🏿‍♂", "🙋‍♀️", "🙋‍♀", "🙋🏻‍♀️", "🙋🏻‍♀", "🙋🏼‍♀️", "🙋🏼‍♀", "🙋🏽‍♀️", "🙋🏽‍♀", "🙋🏾‍♀️", "🙋🏾‍♀", "🙋🏿‍♀️", "🙋🏿‍♀", "🧏", "🧏🏻", "🧏🏼", "🧏🏽", "🧏🏾", "🧏🏿", "🧏‍♂️", "🧏‍♂", "🧏🏻‍♂️", "🧏🏻‍♂", "🧏🏼‍♂️", "🧏🏼‍♂", "🧏🏽‍♂️", "🧏🏽‍♂", "🧏🏾‍♂️", "🧏🏾‍♂", "🧏🏿‍♂️", "🧏🏿‍♂", "🧏‍♀️", "🧏‍♀", "🧏🏻‍♀️", "🧏🏻‍♀", "🧏🏼‍♀️", "🧏🏼‍♀", "🧏🏽‍♀️", "🧏🏽‍♀", "🧏🏾‍♀️", "🧏🏾‍♀", "🧏🏿‍♀️", "🧏🏿‍♀", "🙇", "🙇🏻", "🙇🏼", "🙇🏽", "🙇🏾", "🙇🏿", "🙇‍♂️", "🙇‍♂", "🙇🏻‍♂️", "🙇🏻‍♂", "🙇🏼‍♂️", "🙇🏼‍♂", "🙇🏽‍♂️", "🙇🏽‍♂", "🙇🏾‍♂️", "🙇🏾‍♂", "🙇🏿‍♂️", "🙇🏿‍♂", "🙇‍♀️", "🙇‍♀", "🙇🏻‍♀️", "🙇🏻‍♀", "🙇🏼‍♀️", "🙇🏼‍♀", "🙇🏽‍♀️", "🙇🏽‍♀", "🙇🏾‍♀️", "🙇🏾‍♀", "🙇🏿‍♀️", "🙇🏿‍♀", "🤦", "🤦🏻", "🤦🏼", "🤦🏽", "🤦🏾", "🤦🏿", "🤦‍♂️", "🤦‍♂", "🤦🏻‍♂️", "🤦🏻‍♂", "🤦🏼‍♂️", "🤦🏼‍♂", "🤦🏽‍♂️", "🤦🏽‍♂", "🤦🏾‍♂️", "🤦🏾‍♂", "🤦🏿‍♂️", "🤦🏿‍♂", "🤦‍♀️", "🤦‍♀", "🤦🏻‍♀️", "🤦🏻‍♀", "🤦🏼‍♀️", "🤦🏼‍♀", "🤦🏽‍♀️", "🤦🏽‍♀", "🤦🏾‍♀️", "🤦🏾‍♀", "🤦🏿‍♀️", "🤦🏿‍♀", "🤷", "🤷🏻", "🤷🏼", "🤷🏽", "🤷🏾", "🤷🏿", "🤷‍♂️", "🤷‍♂", "🤷🏻‍♂️", "🤷🏻‍♂", "🤷🏼‍♂️", "🤷🏼‍♂", "🤷🏽‍♂️", "🤷🏽‍♂", "🤷🏾‍♂️", "🤷🏾‍♂", "🤷🏿‍♂️", "🤷🏿‍♂", "🤷‍♀️", "🤷‍♀", "🤷🏻‍♀️", "🤷🏻‍♀", "🤷🏼‍♀️", "🤷🏼‍♀", "🤷🏽‍♀️", "🤷🏽‍♀", "🤷🏾‍♀️", "🤷🏾‍♀", "🤷🏿‍♀️", "🤷🏿‍♀", "🧑‍⚕️", "🧑‍⚕", "🧑🏻‍⚕️", "🧑🏻‍⚕", "🧑🏼‍⚕️", "🧑🏼‍⚕", "🧑🏽‍⚕️", "🧑🏽‍⚕", "🧑🏾‍⚕️", "🧑🏾‍⚕", "🧑🏿‍⚕️", "🧑🏿‍⚕", "👨‍⚕️", "👨‍⚕", "👨🏻‍⚕️", "👨🏻‍⚕", "👨🏼‍⚕️", "👨🏼‍⚕", "👨🏽‍⚕️", "👨🏽‍⚕", "👨🏾‍⚕️", "👨🏾‍⚕", "👨🏿‍⚕️", "👨🏿‍⚕", "👩‍⚕️", "👩‍⚕", "👩🏻‍⚕️", "👩🏻‍⚕", "👩🏼‍⚕️", "👩🏼‍⚕", "👩🏽‍⚕️", "👩🏽‍⚕", "👩🏾‍⚕️", "👩🏾‍⚕", "👩🏿‍⚕️", "👩🏿‍⚕", "🧑‍🎓", "🧑🏻‍🎓", "🧑🏼‍🎓", "🧑🏽‍🎓", "🧑🏾‍🎓", "🧑🏿‍🎓", "👨‍🎓", "👨🏻‍🎓", "👨🏼‍🎓", "👨🏽‍🎓", "👨🏾‍🎓", "👨🏿‍🎓", "👩‍🎓", "👩🏻‍🎓", "👩🏼‍🎓", "👩🏽‍🎓", "👩🏾‍🎓", "👩🏿‍🎓", "🧑‍🏫", "🧑🏻‍🏫", "🧑🏼‍🏫", "🧑🏽‍🏫", "🧑🏾‍🏫", "🧑🏿‍🏫", "👨‍🏫", "👨🏻‍🏫", "👨🏼‍🏫", "👨🏽‍🏫", "👨🏾‍🏫", "👨🏿‍🏫", "👩‍🏫", "👩🏻‍🏫", "👩🏼‍🏫", "👩🏽‍🏫", "👩🏾‍🏫", "👩🏿‍🏫", "🧑‍⚖️", "🧑‍⚖", "🧑🏻‍⚖️", "🧑🏻‍⚖", "🧑🏼‍⚖️", "🧑🏼‍⚖", "🧑🏽‍⚖️", "🧑🏽‍⚖", "🧑🏾‍⚖️", "🧑🏾‍⚖", "🧑🏿‍⚖️", "🧑🏿‍⚖", "👨‍⚖️", "👨‍⚖", "👨🏻‍⚖️", "👨🏻‍⚖", "👨🏼‍⚖️", "👨🏼‍⚖", "👨🏽‍⚖️", "👨🏽‍⚖", "👨🏾‍⚖️", "👨🏾‍⚖", "👨🏿‍⚖️", "👨🏿‍⚖", "👩‍⚖️", "👩‍⚖", "👩🏻‍⚖️", "👩🏻‍⚖", "👩🏼‍⚖️", "👩🏼‍⚖", "👩🏽‍⚖️", "👩🏽‍⚖", "👩🏾‍⚖️", "👩🏾‍⚖", "👩🏿‍⚖️", "👩🏿‍⚖", "🧑‍🌾", "🧑🏻‍🌾", "🧑🏼‍🌾", "🧑🏽‍🌾", "🧑🏾‍🌾", "🧑🏿‍🌾", "👨‍🌾", "👨🏻‍🌾", "👨🏼‍🌾", "👨🏽‍🌾", "👨🏾‍🌾", "👨🏿‍🌾", "👩‍🌾", "👩🏻‍🌾", "👩🏼‍🌾", "👩🏽‍🌾", "👩🏾‍🌾", "👩🏿‍🌾", "🧑‍🍳", "🧑🏻‍🍳", "🧑🏼‍🍳", "🧑🏽‍🍳", "🧑🏾‍🍳", "🧑🏿‍🍳", "👨‍🍳", "👨🏻‍🍳", "👨🏼‍🍳", "👨🏽‍🍳", "👨🏾‍🍳", "👨🏿‍🍳", "👩‍🍳", "👩🏻‍🍳", "👩🏼‍🍳", "👩🏽‍🍳", "👩🏾‍🍳", "👩🏿‍🍳", "🧑‍🔧", "🧑🏻‍🔧", "🧑🏼‍🔧", "🧑🏽‍🔧", "🧑🏾‍🔧", "🧑🏿‍🔧", "👨‍🔧", "👨🏻‍🔧", "👨🏼‍🔧", "👨🏽‍🔧", "👨🏾‍🔧", "👨🏿‍🔧", "👩‍🔧", "👩🏻‍🔧", "👩🏼‍🔧", "👩🏽‍🔧", "👩🏾‍🔧", "👩🏿‍🔧", "🧑‍🏭", "🧑🏻‍🏭", "🧑🏼‍🏭", "🧑🏽‍🏭", "🧑🏾‍🏭", "🧑🏿‍🏭", "👨‍🏭", "👨🏻‍🏭", "👨🏼‍🏭", "👨🏽‍🏭", "👨🏾‍🏭", "👨🏿‍🏭", "👩‍🏭", "👩🏻‍🏭", "👩🏼‍🏭", "👩🏽‍🏭", "👩🏾‍🏭", "👩🏿‍🏭", "🧑‍💼", "🧑🏻‍💼", "🧑🏼‍💼", "🧑🏽‍💼", "🧑🏾‍💼", "🧑🏿‍💼", "👨‍💼", "👨🏻‍💼", "👨🏼‍💼", "👨🏽‍💼", "👨🏾‍💼", "👨🏿‍💼", "👩‍💼", "👩🏻‍💼", "👩🏼‍💼", "👩🏽‍💼", "👩🏾‍💼", "👩🏿‍💼", "🧑‍🔬", "🧑🏻‍🔬", "🧑🏼‍🔬", "🧑🏽‍🔬", "🧑🏾‍🔬", "🧑🏿‍🔬", "👨‍🔬", "👨🏻‍🔬", "👨🏼‍🔬", "👨🏽‍🔬", "👨🏾‍🔬", "👨🏿‍🔬", "👩‍🔬", "👩🏻‍🔬", "👩🏼‍🔬", "👩🏽‍🔬", "👩🏾‍🔬", "👩🏿‍🔬", "🧑‍💻", "🧑🏻‍💻", "🧑🏼‍💻", "🧑🏽‍💻", "🧑🏾‍💻", "🧑🏿‍💻", "👨‍💻", "👨🏻‍💻", "👨🏼‍💻", "👨🏽‍💻", "👨🏾‍💻", "👨🏿‍💻", "👩‍💻", "👩🏻‍💻", "👩🏼‍💻", "👩🏽‍💻", "👩🏾‍💻", "👩🏿‍💻", "🧑‍🎤", "🧑🏻‍🎤", "🧑🏼‍🎤", "🧑🏽‍🎤", "🧑🏾‍🎤", "🧑🏿‍🎤", "👨‍🎤", "👨🏻‍🎤", "👨🏼‍🎤", "👨🏽‍🎤", "👨🏾‍🎤", "👨🏿‍🎤", "👩‍🎤", "👩🏻‍🎤", "👩🏼‍🎤", "👩🏽‍🎤", "👩🏾‍🎤", "👩🏿‍🎤", "🧑‍🎨", "🧑🏻‍🎨", "🧑🏼‍🎨", "🧑🏽‍🎨", "🧑🏾‍🎨", "🧑🏿‍🎨", "👨‍🎨", "👨🏻‍🎨", "👨🏼‍🎨", "👨🏽‍🎨", "👨🏾‍🎨", "👨🏿‍🎨", "👩‍🎨", "👩🏻‍🎨", "👩🏼‍🎨", "👩🏽‍🎨", "👩🏾‍🎨", "👩🏿‍🎨", "🧑‍✈️", "🧑‍✈", "🧑🏻‍✈️", "🧑🏻‍✈", "🧑🏼‍✈️", "🧑🏼‍✈", "🧑🏽‍✈️", "🧑🏽‍✈", "🧑🏾‍✈️", "🧑🏾‍✈", "🧑🏿‍✈️", "🧑🏿‍✈", "👨‍✈️", "👨‍✈", "👨🏻‍✈️", "👨🏻‍✈", "👨🏼‍✈️", "👨🏼‍✈", "👨🏽‍✈️", "👨🏽‍✈", "👨🏾‍✈️", "👨🏾‍✈", "👨🏿‍✈️", "👨🏿‍✈", "👩‍✈️", "👩‍✈", "👩🏻‍✈️", "👩🏻‍✈", "👩🏼‍✈️", "👩🏼‍✈", "👩🏽‍✈️", "👩🏽‍✈", "👩🏾‍✈️", "👩🏾‍✈", "👩🏿‍✈️", "👩🏿‍✈", "🧑‍🚀", "🧑🏻‍🚀", "🧑🏼‍🚀", "🧑🏽‍🚀", "🧑🏾‍🚀", "🧑🏿‍🚀", "👨‍🚀", "👨🏻‍🚀", "👨🏼‍🚀", "👨🏽‍🚀", "👨🏾‍🚀", "👨🏿‍🚀", "👩‍🚀", "👩🏻‍🚀", "👩🏼‍🚀", "👩🏽‍🚀", "👩🏾‍🚀", "👩🏿‍🚀", "🧑‍🚒", "🧑🏻‍🚒", "🧑🏼‍🚒", "🧑🏽‍🚒", "🧑🏾‍🚒", "🧑🏿‍🚒", "👨‍🚒", "👨🏻‍🚒", "👨🏼‍🚒", "👨🏽‍🚒", "👨🏾‍🚒", "👨🏿‍🚒", "👩‍🚒", "👩🏻‍🚒", "👩🏼‍🚒", "👩🏽‍🚒", "👩🏾‍🚒", "👩🏿‍🚒", "👮", "👮🏻", "👮🏼", "👮🏽", "👮🏾", "👮🏿", "👮‍♂️", "👮‍♂", "👮🏻‍♂️", "👮🏻‍♂", "👮🏼‍♂️", "👮🏼‍♂", "👮🏽‍♂️", "👮🏽‍♂", "👮🏾‍♂️", "👮🏾‍♂", "👮🏿‍♂️", "👮🏿‍♂", "👮‍♀️", "👮‍♀", "👮🏻‍♀️", "👮🏻‍♀", "👮🏼‍♀️", "👮🏼‍♀", "👮🏽‍♀️", "👮🏽‍♀", "👮🏾‍♀️", "👮🏾‍♀", "👮🏿‍♀️", "👮🏿‍♀", "🕵️", "🕵", "🕵🏻", "🕵🏼", "🕵🏽", "🕵🏾", "🕵🏿", "🕵️‍♂️", "🕵‍♂️", "🕵️‍♂", "🕵‍♂", "🕵🏻‍♂️", "🕵🏻‍♂", "🕵🏼‍♂️", "🕵🏼‍♂", "🕵🏽‍♂️", "🕵🏽‍♂", "🕵🏾‍♂️", "🕵🏾‍♂", "🕵🏿‍♂️", "🕵🏿‍♂", "🕵️‍♀️", "🕵‍♀️", "🕵️‍♀", "🕵‍♀", "🕵🏻‍♀️", "🕵🏻‍♀", "🕵🏼‍♀️", "🕵🏼‍♀", "🕵🏽‍♀️", "🕵🏽‍♀", "🕵🏾‍♀️", "🕵🏾‍♀", "🕵🏿‍♀️", "🕵🏿‍♀", "💂", "💂🏻", "💂🏼", "💂🏽", "💂🏾", "💂🏿", "💂‍♂️", "💂‍♂", "💂🏻‍♂️", "💂🏻‍♂", "💂🏼‍♂️", "💂🏼‍♂", "💂🏽‍♂️", "💂🏽‍♂", "💂🏾‍♂️", "💂🏾‍♂", "💂🏿‍♂️", "💂🏿‍♂", "💂‍♀️", "💂‍♀", "💂🏻‍♀️", "💂🏻‍♀", "💂🏼‍♀️", "💂🏼‍♀", "💂🏽‍♀️", "💂🏽‍♀", "💂🏾‍♀️", "💂🏾‍♀", "💂🏿‍♀️", "💂🏿‍♀", "🥷", "🥷🏻", "🥷🏼", "🥷🏽", "🥷🏾", "🥷🏿", "👷", "👷🏻", "👷🏼", "👷🏽", "👷🏾", "👷🏿", "👷‍♂️", "👷‍♂", "👷🏻‍♂️", "👷🏻‍♂", "👷🏼‍♂️", "👷🏼‍♂", "👷🏽‍♂️", "👷🏽‍♂", "👷🏾‍♂️", "👷🏾‍♂", "👷🏿‍♂️", "👷🏿‍♂", "👷‍♀️", "👷‍♀", "👷🏻‍♀️", "👷🏻‍♀", "👷🏼‍♀️", "👷🏼‍♀", "👷🏽‍♀️", "👷🏽‍♀", "👷🏾‍♀️", "👷🏾‍♀", "👷🏿‍♀️", "👷🏿‍♀", "🤴", "🤴🏻", "🤴🏼", "🤴🏽", "🤴🏾", "🤴🏿", "👸", "👸🏻", "👸🏼", "👸🏽", "👸🏾", "👸🏿", "👳", "👳🏻", "👳🏼", "👳🏽", "👳🏾", "👳🏿", "👳‍♂️", "👳‍♂", "👳🏻‍♂️", "👳🏻‍♂", "👳🏼‍♂️", "👳🏼‍♂", "👳🏽‍♂️", "👳🏽‍♂", "👳🏾‍♂️", "👳🏾‍♂", "👳🏿‍♂️", "👳🏿‍♂", "👳‍♀️", "👳‍♀", "👳🏻‍♀️", "👳🏻‍♀", "👳🏼‍♀️", "👳🏼‍♀", "👳🏽‍♀️", "👳🏽‍♀", "👳🏾‍♀️", "👳🏾‍♀", "👳🏿‍♀️", "👳🏿‍♀", "👲", "👲🏻", "👲🏼", "👲🏽", "👲🏾", "👲🏿", "🧕", "🧕🏻", "🧕🏼", "🧕🏽", "🧕🏾", "🧕🏿", "🤵", "🤵🏻", "🤵🏼", "🤵🏽", "🤵🏾", "🤵🏿", "🤵‍♂️", "🤵‍♂", "🤵🏻‍♂️", "🤵🏻‍♂", "🤵🏼‍♂️", "🤵🏼‍♂", "🤵🏽‍♂️", "🤵🏽‍♂", "🤵🏾‍♂️", "🤵🏾‍♂", "🤵🏿‍♂️", "🤵🏿‍♂", "🤵‍♀️", "🤵‍♀", "🤵🏻‍♀️", "🤵🏻‍♀", "🤵🏼‍♀️", "🤵🏼‍♀", "🤵🏽‍♀️", "🤵🏽‍♀", "🤵🏾‍♀️", "🤵🏾‍♀", "🤵🏿‍♀️", "🤵🏿‍♀", "👰", "👰🏻", "👰🏼", "👰🏽", "👰🏾", "👰🏿", "👰‍♂️", "👰‍♂", "👰🏻‍♂️", "👰🏻‍♂", "👰🏼‍♂️", "👰🏼‍♂", "👰🏽‍♂️", "👰🏽‍♂", "👰🏾‍♂️", "👰🏾‍♂", "👰🏿‍♂️", "👰🏿‍♂", "👰‍♀️", "👰‍♀", "👰🏻‍♀️", "👰🏻‍♀", "👰🏼‍♀️", "👰🏼‍♀", "👰🏽‍♀️", "👰🏽‍♀", "👰🏾‍♀️", "👰🏾‍♀", "👰🏿‍♀️", "👰🏿‍♀", "🤰", "🤰🏻", "🤰🏼", "🤰🏽", "🤰🏾", "🤰🏿", "🤱", "🤱🏻", "🤱🏼", "🤱🏽", "🤱🏾", "🤱🏿", "👩‍🍼", "👩🏻‍🍼", "👩🏼‍🍼", "👩🏽‍🍼", "👩🏾‍🍼", "👩🏿‍🍼", "👨‍🍼", "👨🏻‍🍼", "👨🏼‍🍼", "👨🏽‍🍼", "👨🏾‍🍼", "👨🏿‍🍼", "🧑‍🍼", "🧑🏻‍🍼", "🧑🏼‍🍼", "🧑🏽‍🍼", "🧑🏾‍🍼", "🧑🏿‍🍼", "👼", "👼🏻", "👼🏼", "👼🏽", "👼🏾", "👼🏿", "🎅", "🎅🏻", "🎅🏼", "🎅🏽", "🎅🏾", "🎅🏿", "🤶", "🤶🏻", "🤶🏼", "🤶🏽", "🤶🏾", "🤶🏿", "🧑‍🎄", "🧑🏻‍🎄", "🧑🏼‍🎄", "🧑🏽‍🎄", "🧑🏾‍🎄", "🧑🏿‍🎄", "🦸", "🦸🏻", "🦸🏼", "🦸🏽", "🦸🏾", "🦸🏿", "🦸‍♂️", "🦸‍♂", "🦸🏻‍♂️", "🦸🏻‍♂", "🦸🏼‍♂️", "🦸🏼‍♂", "🦸🏽‍♂️", "🦸🏽‍♂", "🦸🏾‍♂️", "🦸🏾‍♂", "🦸🏿‍♂️", "🦸🏿‍♂", "🦸‍♀️", "🦸‍♀", "🦸🏻‍♀️", "🦸🏻‍♀", "🦸🏼‍♀️", "🦸🏼‍♀", "🦸🏽‍♀️", "🦸🏽‍♀", "🦸🏾‍♀️", "🦸🏾‍♀", "🦸🏿‍♀️", "🦸🏿‍♀", "🦹", "🦹🏻", "🦹🏼", "🦹🏽", "🦹🏾", "🦹🏿", "🦹‍♂️", "🦹‍♂", "🦹🏻‍♂️", "🦹🏻‍♂", "🦹🏼‍♂️", "🦹🏼‍♂", "🦹🏽‍♂️", "🦹🏽‍♂", "🦹🏾‍♂️", "🦹🏾‍♂", "🦹🏿‍♂️", "🦹🏿‍♂", "🦹‍♀️", "🦹‍♀", "🦹🏻‍♀️", "🦹🏻‍♀", "🦹🏼‍♀️", "🦹🏼‍♀", "🦹🏽‍♀️", "🦹🏽‍♀", "🦹🏾‍♀️", "🦹🏾‍♀", "🦹🏿‍♀️", "🦹🏿‍♀", "🧙", "🧙🏻", "🧙🏼", "🧙🏽", "🧙🏾", "🧙🏿", "🧙‍♂️", "🧙‍♂", "🧙🏻‍♂️", "🧙🏻‍♂", "🧙🏼‍♂️", "🧙🏼‍♂", "🧙🏽‍♂️", "🧙🏽‍♂", "🧙🏾‍♂️", "🧙🏾‍♂", "🧙🏿‍♂️", "🧙🏿‍♂", "🧙‍♀️", "🧙‍♀", "🧙🏻‍♀️", "🧙🏻‍♀", "🧙🏼‍♀️", "🧙🏼‍♀", "🧙🏽‍♀️", "🧙🏽‍♀", "🧙🏾‍♀️", "🧙🏾‍♀", "🧙🏿‍♀️", "🧙🏿‍♀", "🧚", "🧚🏻", "🧚🏼", "🧚🏽", "🧚🏾", "🧚🏿", "🧚‍♂️", "🧚‍♂", "🧚🏻‍♂️", "🧚🏻‍♂", "🧚🏼‍♂️", "🧚🏼‍♂", "🧚🏽‍♂️", "🧚🏽‍♂", "🧚🏾‍♂️", "🧚🏾‍♂", "🧚🏿‍♂️", "🧚🏿‍♂", "🧚‍♀️", "🧚‍♀", "🧚🏻‍♀️", "🧚🏻‍♀", "🧚🏼‍♀️", "🧚🏼‍♀", "🧚🏽‍♀️", "🧚🏽‍♀", "🧚🏾‍♀️", "🧚🏾‍♀", "🧚🏿‍♀️", "🧚🏿‍♀", "🧛", "🧛🏻", "🧛🏼", "🧛🏽", "🧛🏾", "🧛🏿", "🧛‍♂️", "🧛‍♂", "🧛🏻‍♂️", "🧛🏻‍♂", "🧛🏼‍♂️", "🧛🏼‍♂", "🧛🏽‍♂️", "🧛🏽‍♂", "🧛🏾‍♂️", "🧛🏾‍♂", "🧛🏿‍♂️", "🧛🏿‍♂", "🧛‍♀️", "🧛‍♀", "🧛🏻‍♀️", "🧛🏻‍♀", "🧛🏼‍♀️", "🧛🏼‍♀", "🧛🏽‍♀️", "🧛🏽‍♀", "🧛🏾‍♀️", "🧛🏾‍♀", "🧛🏿‍♀️", "🧛🏿‍♀", "🧜", "🧜🏻", "🧜🏼", "🧜🏽", "🧜🏾", "🧜🏿", "🧜‍♂️", "🧜‍♂", "🧜🏻‍♂️", "🧜🏻‍♂", "🧜🏼‍♂️", "🧜🏼‍♂", "🧜🏽‍♂️", "🧜🏽‍♂", "🧜🏾‍♂️", "🧜🏾‍♂", "🧜🏿‍♂️", "🧜🏿‍♂", "🧜‍♀️", "🧜‍♀", "🧜🏻‍♀️", "🧜🏻‍♀", "🧜🏼‍♀️", "🧜🏼‍♀", "🧜🏽‍♀️", "🧜🏽‍♀", "🧜🏾‍♀️", "🧜🏾‍♀", "🧜🏿‍♀️", "🧜🏿‍♀", "🧝", "🧝🏻", "🧝🏼", "🧝🏽", "🧝🏾", "🧝🏿", "🧝‍♂️", "🧝‍♂", "🧝🏻‍♂️", "🧝🏻‍♂", "🧝🏼‍♂️", "🧝🏼‍♂", "🧝🏽‍♂️", "🧝🏽‍♂", "🧝🏾‍♂️", "🧝🏾‍♂", "🧝🏿‍♂️", "🧝🏿‍♂", "🧝‍♀️", "🧝‍♀", "🧝🏻‍♀️", "🧝🏻‍♀", "🧝🏼‍♀️", "🧝🏼‍♀", "🧝🏽‍♀️", "🧝🏽‍♀", "🧝🏾‍♀️", "🧝🏾‍♀", "🧝🏿‍♀️", "🧝🏿‍♀", "🧞", "🧞‍♂️", "🧞‍♂", "🧞‍♀️", "🧞‍♀", "🧟", "🧟‍♂️", "🧟‍♂", "🧟‍♀️", "🧟‍♀", "💆", "💆🏻", "💆🏼", "💆🏽", "💆🏾", "💆🏿", "💆‍♂️", "💆‍♂", "💆🏻‍♂️", "💆🏻‍♂", "💆🏼‍♂️", "💆🏼‍♂", "💆🏽‍♂️", "💆🏽‍♂", "💆🏾‍♂️", "💆🏾‍♂", "💆🏿‍♂️", "💆🏿‍♂", "💆‍♀️", "💆‍♀", "💆🏻‍♀️", "💆🏻‍♀", "💆🏼‍♀️", "💆🏼‍♀", "💆🏽‍♀️", "💆🏽‍♀", "💆🏾‍♀️", "💆🏾‍♀", "💆🏿‍♀️", "💆🏿‍♀", "💇", "💇🏻", "💇🏼", "💇🏽", "💇🏾", "💇🏿", "💇‍♂️", "💇‍♂", "💇🏻‍♂️", "💇🏻‍♂", "💇🏼‍♂️", "💇🏼‍♂", "💇🏽‍♂️", "💇🏽‍♂", "💇🏾‍♂️", "💇🏾‍♂", "💇🏿‍♂️", "💇🏿‍♂", "💇‍♀️", "💇‍♀", "💇🏻‍♀️", "💇🏻‍♀", "💇🏼‍♀️", "💇🏼‍♀", "💇🏽‍♀️", "💇🏽‍♀", "💇🏾‍♀️", "💇🏾‍♀", "💇🏿‍♀️", "💇🏿‍♀", "🚶", "🚶🏻", "🚶🏼", "🚶🏽", "🚶🏾", "🚶🏿", "🚶‍♂️", "🚶‍♂", "🚶🏻‍♂️", "🚶🏻‍♂", "🚶🏼‍♂️", "🚶🏼‍♂", "🚶🏽‍♂️", "🚶🏽‍♂", "🚶🏾‍♂️", "🚶🏾‍♂", "🚶🏿‍♂️", "🚶🏿‍♂", "🚶‍♀️", "🚶‍♀", "🚶🏻‍♀️", "🚶🏻‍♀", "🚶🏼‍♀️", "🚶🏼‍♀", "🚶🏽‍♀️", "🚶🏽‍♀", "🚶🏾‍♀️", "🚶🏾‍♀", "🚶🏿‍♀️", "🚶🏿‍♀", "🧍", "🧍🏻", "🧍🏼", "🧍🏽", "🧍🏾", "🧍🏿", "🧍‍♂️", "🧍‍♂", "🧍🏻‍♂️", "🧍🏻‍♂", "🧍🏼‍♂️", "🧍🏼‍♂", "🧍🏽‍♂️", "🧍🏽‍♂", "🧍🏾‍♂️", "🧍🏾‍♂", "🧍🏿‍♂️", "🧍🏿‍♂", "🧍‍♀️", "🧍‍♀", "🧍🏻‍♀️", "🧍🏻‍♀", "🧍🏼‍♀️", "🧍🏼‍♀", "🧍🏽‍♀️", "🧍🏽‍♀", "🧍🏾‍♀️", "🧍🏾‍♀", "🧍🏿‍♀️", "🧍🏿‍♀", "🧎", "🧎🏻", "🧎🏼", "🧎🏽", "🧎🏾", "🧎🏿", "🧎‍♂️", "🧎‍♂", "🧎🏻‍♂️", "🧎🏻‍♂", "🧎🏼‍♂️", "🧎🏼‍♂", "🧎🏽‍♂️", "🧎🏽‍♂", "🧎🏾‍♂️", "🧎🏾‍♂", "🧎🏿‍♂️", "🧎🏿‍♂", "🧎‍♀️", "🧎‍♀", "🧎🏻‍♀️", "🧎🏻‍♀", "🧎🏼‍♀️", "🧎🏼‍♀", "🧎🏽‍♀️", "🧎🏽‍♀", "🧎🏾‍♀️", "🧎🏾‍♀", "🧎🏿‍♀️", "🧎🏿‍♀", "🧑‍🦯", "🧑🏻‍🦯", "🧑🏼‍🦯", "🧑🏽‍🦯", "🧑🏾‍🦯", "🧑🏿‍🦯", "👨‍🦯", "👨🏻‍🦯", "👨🏼‍🦯", "👨🏽‍🦯", "👨🏾‍🦯", "👨🏿‍🦯", "👩‍🦯", "👩🏻‍🦯", "👩🏼‍🦯", "👩🏽‍🦯", "👩🏾‍🦯", "👩🏿‍🦯", "🧑‍🦼", "🧑🏻‍🦼", "🧑🏼‍🦼", "🧑🏽‍🦼", "🧑🏾‍🦼", "🧑🏿‍🦼", "👨‍🦼", "👨🏻‍🦼", "👨🏼‍🦼", "👨🏽‍🦼", "👨🏾‍🦼", "👨🏿‍🦼", "👩‍🦼", "👩🏻‍🦼", "👩🏼‍🦼", "👩🏽‍🦼", "👩🏾‍🦼", "👩🏿‍🦼", "🧑‍🦽", "🧑🏻‍🦽", "🧑🏼‍🦽", "🧑🏽‍🦽", "🧑🏾‍🦽", "🧑🏿‍🦽", "👨‍🦽", "👨🏻‍🦽", "👨🏼‍🦽", "👨🏽‍🦽", "👨🏾‍🦽", "👨🏿‍🦽", "👩‍🦽", "👩🏻‍🦽", "👩🏼‍🦽", "👩🏽‍🦽", "👩🏾‍🦽", "👩🏿‍🦽", "🏃", "🏃🏻", "🏃🏼", "🏃🏽", "🏃🏾", "🏃🏿", "🏃‍♂️", "🏃‍♂", "🏃🏻‍♂️", "🏃🏻‍♂", "🏃🏼‍♂️", "🏃🏼‍♂", "🏃🏽‍♂️", "🏃🏽‍♂", "🏃🏾‍♂️", "🏃🏾‍♂", "🏃🏿‍♂️", "🏃🏿‍♂", "🏃‍♀️", "🏃‍♀", "🏃🏻‍♀️", "🏃🏻‍♀", "🏃🏼‍♀️", "🏃🏼‍♀", "🏃🏽‍♀️", "🏃🏽‍♀", "🏃🏾‍♀️", "🏃🏾‍♀", "🏃🏿‍♀️", "🏃🏿‍♀", "💃", "💃🏻", "💃🏼", "💃🏽", "💃🏾", "💃🏿", "🕺", "🕺🏻", "🕺🏼", "🕺🏽", "🕺🏾", "🕺🏿", "🕴️", "🕴", "🕴🏻", "🕴🏼", "🕴🏽", "🕴🏾", "🕴🏿", "👯", "👯‍♂️", "👯‍♂", "👯‍♀️", "👯‍♀", "🧖", "🧖🏻", "🧖🏼", "🧖🏽", "🧖🏾", "🧖🏿", "🧖‍♂️", "🧖‍♂", "🧖🏻‍♂️", "🧖🏻‍♂", "🧖🏼‍♂️", "🧖🏼‍♂", "🧖🏽‍♂️", "🧖🏽‍♂", "🧖🏾‍♂️", "🧖🏾‍♂", "🧖🏿‍♂️", "🧖🏿‍♂", "🧖‍♀️", "🧖‍♀", "🧖🏻‍♀️", "🧖🏻‍♀", "🧖🏼‍♀️", "🧖🏼‍♀", "🧖🏽‍♀️", "🧖🏽‍♀", "🧖🏾‍♀️", "🧖🏾‍♀", "🧖🏿‍♀️", "🧖🏿‍♀", "🧗", "🧗🏻", "🧗🏼", "🧗🏽", "🧗🏾", "🧗🏿", "🧗‍♂️", "🧗‍♂", "🧗🏻‍♂️", "🧗🏻‍♂", "🧗🏼‍♂️", "🧗🏼‍♂", "🧗🏽‍♂️", "🧗🏽‍♂", "🧗🏾‍♂️", "🧗🏾‍♂", "🧗🏿‍♂️", "🧗🏿‍♂", "🧗‍♀️", "🧗‍♀", "🧗🏻‍♀️", "🧗🏻‍♀", "🧗🏼‍♀️", "🧗🏼‍♀", "🧗🏽‍♀️", "🧗🏽‍♀", "🧗🏾‍♀️", "🧗🏾‍♀", "🧗🏿‍♀️", "🧗🏿‍♀", "🤺", "🏇", "🏇🏻", "🏇🏼", "🏇🏽", "🏇🏾", "🏇🏿", "⛷️", "⛷", "🏂", "🏂🏻", "🏂🏼", "🏂🏽", "🏂🏾", "🏂🏿", "🏌️", "🏌", "🏌🏻", "🏌🏼", "🏌🏽", "🏌🏾", "🏌🏿", "🏌️‍♂️", "🏌‍♂️", "🏌️‍♂", "🏌‍♂", "🏌🏻‍♂️", "🏌🏻‍♂", "🏌🏼‍♂️", "🏌🏼‍♂", "🏌🏽‍♂️", "🏌🏽‍♂", "🏌🏾‍♂️", "🏌🏾‍♂", "🏌🏿‍♂️", "🏌🏿‍♂", "🏌️‍♀️", "🏌‍♀️", "🏌️‍♀", "🏌‍♀", "🏌🏻‍♀️", "🏌🏻‍♀", "🏌🏼‍♀️", "🏌🏼‍♀", "🏌🏽‍♀️", "🏌🏽‍♀", "🏌🏾‍♀️", "🏌🏾‍♀", "🏌🏿‍♀️", "🏌🏿‍♀", "🏄", "🏄🏻", "🏄🏼", "🏄🏽", "🏄🏾", "🏄🏿", "🏄‍♂️", "🏄‍♂", "🏄🏻‍♂️", "🏄🏻‍♂", "🏄🏼‍♂️", "🏄🏼‍♂", "🏄🏽‍♂️", "🏄🏽‍♂", "🏄🏾‍♂️", "🏄🏾‍♂", "🏄🏿‍♂️", "🏄🏿‍♂", "🏄‍♀️", "🏄‍♀", "🏄🏻‍♀️", "🏄🏻‍♀", "🏄🏼‍♀️", "🏄🏼‍♀", "🏄🏽‍♀️", "🏄🏽‍♀", "🏄🏾‍♀️", "🏄🏾‍♀", "🏄🏿‍♀️", "🏄🏿‍♀", "🚣", "🚣🏻", "🚣🏼", "🚣🏽", "🚣🏾", "🚣🏿", "🚣‍♂️", "🚣‍♂", "🚣🏻‍♂️", "🚣🏻‍♂", "🚣🏼‍♂️", "🚣🏼‍♂", "🚣🏽‍♂️", "🚣🏽‍♂", "🚣🏾‍♂️", "🚣🏾‍♂", "🚣🏿‍♂️", "🚣🏿‍♂", "🚣‍♀️", "🚣‍♀", "🚣🏻‍♀️", "🚣🏻‍♀", "🚣🏼‍♀️", "🚣🏼‍♀", "🚣🏽‍♀️", "🚣🏽‍♀", "🚣🏾‍♀️", "🚣🏾‍♀", "🚣🏿‍♀️", "🚣🏿‍♀", "🏊", "🏊🏻", "🏊🏼", "🏊🏽", "🏊🏾", "🏊🏿", "🏊‍♂️", "🏊‍♂", "🏊🏻‍♂️", "🏊🏻‍♂", "🏊🏼‍♂️", "🏊🏼‍♂", "🏊🏽‍♂️", "🏊🏽‍♂", "🏊🏾‍♂️", "🏊🏾‍♂", "🏊🏿‍♂️", "🏊🏿‍♂", "🏊‍♀️", "🏊‍♀", "🏊🏻‍♀️", "🏊🏻‍♀", "🏊🏼‍♀️", "🏊🏼‍♀", "🏊🏽‍♀️", "🏊🏽‍♀", "🏊🏾‍♀️", "🏊🏾‍♀", "🏊🏿‍♀️", "🏊🏿‍♀", "⛹️", "⛹", "⛹🏻", "⛹🏼", "⛹🏽", "⛹🏾", "⛹🏿", "⛹️‍♂️", "⛹‍♂️", "⛹️‍♂", "⛹‍♂", "⛹🏻‍♂️", "⛹🏻‍♂", "⛹🏼‍♂️", "⛹🏼‍♂", "⛹🏽‍♂️", "⛹🏽‍♂", "⛹🏾‍♂️", "⛹🏾‍♂", "⛹🏿‍♂️", "⛹🏿‍♂", "⛹️‍♀️", "⛹‍♀️", "⛹️‍♀", "⛹‍♀", "⛹🏻‍♀️", "⛹🏻‍♀", "⛹🏼‍♀️", "⛹🏼‍♀", "⛹🏽‍♀️", "⛹🏽‍♀", "⛹🏾‍♀️", "⛹🏾‍♀", "⛹🏿‍♀️", "⛹🏿‍♀", "🏋️", "🏋", "🏋🏻", "🏋🏼", "🏋🏽", "🏋🏾", "🏋🏿", "🏋️‍♂️", "🏋‍♂️", "🏋️‍♂", "🏋‍♂", "🏋🏻‍♂️", "🏋🏻‍♂", "🏋🏼‍♂️", "🏋🏼‍♂", "🏋🏽‍♂️", "🏋🏽‍♂", "🏋🏾‍♂️", "🏋🏾‍♂", "🏋🏿‍♂️", "🏋🏿‍♂", "🏋️‍♀️", "🏋‍♀️", "🏋️‍♀", "🏋‍♀", "🏋🏻‍♀️", "🏋🏻‍♀", "🏋🏼‍♀️", "🏋🏼‍♀", "🏋🏽‍♀️", "🏋🏽‍♀", "🏋🏾‍♀️", "🏋🏾‍♀", "🏋🏿‍♀️", "🏋🏿‍♀", "🚴", "🚴🏻", "🚴🏼", "🚴🏽", "🚴🏾", "🚴🏿", "🚴‍♂️", "🚴‍♂", "🚴🏻‍♂️", "🚴🏻‍♂", "🚴🏼‍♂️", "🚴🏼‍♂", "🚴🏽‍♂️", "🚴🏽‍♂", "🚴🏾‍♂️", "🚴🏾‍♂", "🚴🏿‍♂️", "🚴🏿‍♂", "🚴‍♀️", "🚴‍♀", "🚴🏻‍♀️", "🚴🏻‍♀", "🚴🏼‍♀️", "🚴🏼‍♀", "🚴🏽‍♀️", "🚴🏽‍♀", "🚴🏾‍♀️", "🚴🏾‍♀", "🚴🏿‍♀️", "🚴🏿‍♀", "🚵", "🚵🏻", "🚵🏼", "🚵🏽", "🚵🏾", "🚵🏿", "🚵‍♂️", "🚵‍♂", "🚵🏻‍♂️", "🚵🏻‍♂", "🚵🏼‍♂️", "🚵🏼‍♂", "🚵🏽‍♂️", "🚵🏽‍♂", "🚵🏾‍♂️", "🚵🏾‍♂", "🚵🏿‍♂️", "🚵🏿‍♂", "🚵‍♀️", "🚵‍♀", "🚵🏻‍♀️", "🚵🏻‍♀", "🚵🏼‍♀️", "🚵🏼‍♀", "🚵🏽‍♀️", "🚵🏽‍♀", "🚵🏾‍♀️", "🚵🏾‍♀", "🚵🏿‍♀️", "🚵🏿‍♀", "🤸", "🤸🏻", "🤸🏼", "🤸🏽", "🤸🏾", "🤸🏿", "🤸‍♂️", "🤸‍♂", "🤸🏻‍♂️", "🤸🏻‍♂", "🤸🏼‍♂️", "🤸🏼‍♂", "🤸🏽‍♂️", "🤸🏽‍♂", "🤸🏾‍♂️", "🤸🏾‍♂", "🤸🏿‍♂️", "🤸🏿‍♂", "🤸‍♀️", "🤸‍♀", "🤸🏻‍♀️", "🤸🏻‍♀", "🤸🏼‍♀️", "🤸🏼‍♀", "🤸🏽‍♀️", "🤸🏽‍♀", "🤸🏾‍♀️", "🤸🏾‍♀", "🤸🏿‍♀️", "🤸🏿‍♀", "🤼", "🤼‍♂️", "🤼‍♂", "🤼‍♀️", "🤼‍♀", "🤽", "🤽🏻", "🤽🏼", "🤽🏽", "🤽🏾", "🤽🏿", "🤽‍♂️", "🤽‍♂", "🤽🏻‍♂️", "🤽🏻‍♂", "🤽🏼‍♂️", "🤽🏼‍♂", "🤽🏽‍♂️", "🤽🏽‍♂", "🤽🏾‍♂️", "🤽🏾‍♂", "🤽🏿‍♂️", "🤽🏿‍♂", "🤽‍♀️", "🤽‍♀", "🤽🏻‍♀️", "🤽🏻‍♀", "🤽🏼‍♀️", "🤽🏼‍♀", "🤽🏽‍♀️", "🤽🏽‍♀", "🤽🏾‍♀️", "🤽🏾‍♀", "🤽🏿‍♀️", "🤽🏿‍♀", "🤾", "🤾🏻", "🤾🏼", "🤾🏽", "🤾🏾", "🤾🏿", "🤾‍♂️", "🤾‍♂", "🤾🏻‍♂️", "🤾🏻‍♂", "🤾🏼‍♂️", "🤾🏼‍♂", "🤾🏽‍♂️", "🤾🏽‍♂", "🤾🏾‍♂️", "🤾🏾‍♂", "🤾🏿‍♂️", "🤾🏿‍♂", "🤾‍♀️", "🤾‍♀", "🤾🏻‍♀️", "🤾🏻‍♀", "🤾🏼‍♀️", "🤾🏼‍♀", "🤾🏽‍♀️", "🤾🏽‍♀", "🤾🏾‍♀️", "🤾🏾‍♀", "🤾🏿‍♀️", "🤾🏿‍♀", "🤹", "🤹🏻", "🤹🏼", "🤹🏽", "🤹🏾", "🤹🏿", "🤹‍♂️", "🤹‍♂", "🤹🏻‍♂️", "🤹🏻‍♂", "🤹🏼‍♂️", "🤹🏼‍♂", "🤹🏽‍♂️", "🤹🏽‍♂", "🤹🏾‍♂️", "🤹🏾‍♂", "🤹🏿‍♂️", "🤹🏿‍♂", "🤹‍♀️", "🤹‍♀", "🤹🏻‍♀️", "🤹🏻‍♀", "🤹🏼‍♀️", "🤹🏼‍♀", "🤹🏽‍♀️", "🤹🏽‍♀", "🤹🏾‍♀️", "🤹🏾‍♀", "🤹🏿‍♀️", "🤹🏿‍♀", "🧘", "🧘🏻", "🧘🏼", "🧘🏽", "🧘🏾", "🧘🏿", "🧘‍♂️", "🧘‍♂", "🧘🏻‍♂️", "🧘🏻‍♂", "🧘🏼‍♂️", "🧘🏼‍♂", "🧘🏽‍♂️", "🧘🏽‍♂", "🧘🏾‍♂️", "🧘🏾‍♂", "🧘🏿‍♂️", "🧘🏿‍♂", "🧘‍♀️", "🧘‍♀", "🧘🏻‍♀️", "🧘🏻‍♀", "🧘🏼‍♀️", "🧘🏼‍♀", "🧘🏽‍♀️", "🧘🏽‍♀", "🧘🏾‍♀️", "🧘🏾‍♀", "🧘🏿‍♀️", "🧘🏿‍♀", "🛀", "🛀🏻", "🛀🏼", "🛀🏽", "🛀🏾", "🛀🏿", "🛌", "🛌🏻", "🛌🏼", "🛌🏽", "🛌🏾", "🛌🏿", "🧑‍🤝‍🧑", "🧑🏻‍🤝‍🧑🏻", "🧑🏻‍🤝‍🧑🏼", "🧑🏻‍🤝‍🧑🏽", "🧑🏻‍🤝‍🧑🏾", "🧑🏻‍🤝‍🧑🏿", "🧑🏼‍🤝‍🧑🏻", "🧑🏼‍🤝‍🧑🏼", "🧑🏼‍🤝‍🧑🏽", "🧑🏼‍🤝‍🧑🏾", "🧑🏼‍🤝‍🧑🏿", "🧑🏽‍🤝‍🧑🏻", "🧑🏽‍🤝‍🧑🏼", "🧑🏽‍🤝‍🧑🏽", "🧑🏽‍🤝‍🧑🏾", "🧑🏽‍🤝‍🧑🏿", "🧑🏾‍🤝‍🧑🏻", "🧑🏾‍🤝‍🧑🏼", "🧑🏾‍🤝‍🧑🏽", "🧑🏾‍🤝‍🧑🏾", "🧑🏾‍🤝‍🧑🏿", "🧑🏿‍🤝‍🧑🏻", "🧑🏿‍🤝‍🧑🏼", "🧑🏿‍🤝‍🧑🏽", "🧑🏿‍🤝‍🧑🏾", "🧑🏿‍🤝‍🧑🏿", "👭", "👭🏻", "👩🏻‍🤝‍👩🏼", "👩🏻‍🤝‍👩🏽", "👩🏻‍🤝‍👩🏾", "👩🏻‍🤝‍👩🏿", "👩🏼‍🤝‍👩🏻", "👭🏼", "👩🏼‍🤝‍👩🏽", "👩🏼‍🤝‍👩🏾", "👩🏼‍🤝‍👩🏿", "👩🏽‍🤝‍👩🏻", "👩🏽‍🤝‍👩🏼", "👭🏽", "👩🏽‍🤝‍👩🏾", "👩🏽‍🤝‍👩🏿", "👩🏾‍🤝‍👩🏻", "👩🏾‍🤝‍👩🏼", "👩🏾‍🤝‍👩🏽", "👭🏾", "👩🏾‍🤝‍👩🏿", "👩🏿‍🤝‍👩🏻", "👩🏿‍🤝‍👩🏼", "👩🏿‍🤝‍👩🏽", "👩🏿‍🤝‍👩🏾", "👭🏿", "👫", "👫🏻", "👩🏻‍🤝‍👨🏼", "👩🏻‍🤝‍👨🏽", "👩🏻‍🤝‍👨🏾", "👩🏻‍🤝‍👨🏿", "👩🏼‍🤝‍👨🏻", "👫🏼", "👩🏼‍🤝‍👨🏽", "👩🏼‍🤝‍👨🏾", "👩🏼‍🤝‍👨🏿", "👩🏽‍🤝‍👨🏻", "👩🏽‍🤝‍👨🏼", "👫🏽", "👩🏽‍🤝‍👨🏾", "👩🏽‍🤝‍👨🏿", "👩🏾‍🤝‍👨🏻", "👩🏾‍🤝‍👨🏼", "👩🏾‍🤝‍👨🏽", "👫🏾", "👩🏾‍🤝‍👨🏿", "👩🏿‍🤝‍👨🏻", "👩🏿‍🤝‍👨🏼", "👩🏿‍🤝‍👨🏽", "👩🏿‍🤝‍👨🏾", "👫🏿", "👬", "👬🏻", "👨🏻‍🤝‍👨🏼", "👨🏻‍🤝‍👨🏽", "👨🏻‍🤝‍👨🏾", "👨🏻‍🤝‍👨🏿", "👨🏼‍🤝‍👨🏻", "👬🏼", "👨🏼‍🤝‍👨🏽", "👨🏼‍🤝‍👨🏾", "👨🏼‍🤝‍👨🏿", "👨🏽‍🤝‍👨🏻", "👨🏽‍🤝‍👨🏼", "👬🏽", "👨🏽‍🤝‍👨🏾", "👨🏽‍🤝‍👨🏿", "👨🏾‍🤝‍👨🏻", "👨🏾‍🤝‍👨🏼", "👨🏾‍🤝‍👨🏽", "👬🏾", "👨🏾‍🤝‍👨🏿", "👨🏿‍🤝‍👨🏻", "👨🏿‍🤝‍👨🏼", "👨🏿‍🤝‍👨🏽", "👨🏿‍🤝‍👨🏾", "👬🏿", "💏", "💏🏻", "💏🏼", "💏🏽", "💏🏾", "💏🏿", "🧑🏻‍❤️‍💋‍🧑🏼", "🧑🏻‍❤‍💋‍🧑🏼", "🧑🏻‍❤️‍💋‍🧑🏽", "🧑🏻‍❤‍💋‍🧑🏽", "🧑🏻‍❤️‍💋‍🧑🏾", "🧑🏻‍❤‍💋‍🧑🏾", "🧑🏻‍❤️‍💋‍🧑🏿", "🧑🏻‍❤‍💋‍🧑🏿", "🧑🏼‍❤️‍💋‍🧑🏻", "🧑🏼‍❤‍💋‍🧑🏻", "🧑🏼‍❤️‍💋‍🧑🏽", "🧑🏼‍❤‍💋‍🧑🏽", "🧑🏼‍❤️‍💋‍🧑🏾", "🧑🏼‍❤‍💋‍🧑🏾", "🧑🏼‍❤️‍💋‍🧑🏿", "🧑🏼‍❤‍💋‍🧑🏿", "🧑🏽‍❤️‍💋‍🧑🏻", "🧑🏽‍❤‍💋‍🧑🏻", "🧑🏽‍❤️‍💋‍🧑🏼", "🧑🏽‍❤‍💋‍🧑🏼", "🧑🏽‍❤️‍💋‍🧑🏾", "🧑🏽‍❤‍💋‍🧑🏾", "🧑🏽‍❤️‍💋‍🧑🏿", "🧑🏽‍❤‍💋‍🧑🏿", "🧑🏾‍❤️‍💋‍🧑🏻", "🧑🏾‍❤‍💋‍🧑🏻", "🧑🏾‍❤️‍💋‍🧑🏼", "🧑🏾‍❤‍💋‍🧑🏼", "🧑🏾‍❤️‍💋‍🧑🏽", "🧑🏾‍❤‍💋‍🧑🏽", "🧑🏾‍❤️‍💋‍🧑🏿", "🧑🏾‍❤‍💋‍🧑🏿", "🧑🏿‍❤️‍💋‍🧑🏻", "🧑🏿‍❤‍💋‍🧑🏻", "🧑🏿‍❤️‍💋‍🧑🏼", "🧑🏿‍❤‍💋‍🧑🏼", "🧑🏿‍❤️‍💋‍🧑🏽", "🧑🏿‍❤‍💋‍🧑🏽", "🧑🏿‍❤️‍💋‍🧑🏾", "🧑🏿‍❤‍💋‍🧑🏾", "👩‍❤️‍💋‍👨", "👩‍❤‍💋‍👨", "👩🏻‍❤️‍💋‍👨🏻", "👩🏻‍❤‍💋‍👨🏻", "👩🏻‍❤️‍💋‍👨🏼", "👩🏻‍❤‍💋‍👨🏼", "👩🏻‍❤️‍💋‍👨🏽", "👩🏻‍❤‍💋‍👨🏽", "👩🏻‍❤️‍💋‍👨🏾", "👩🏻‍❤‍💋‍👨🏾", "👩🏻‍❤️‍💋‍👨🏿", "👩🏻‍❤‍💋‍👨🏿", "👩🏼‍❤️‍💋‍👨🏻", "👩🏼‍❤‍💋‍👨🏻", "👩🏼‍❤️‍💋‍👨🏼", "👩🏼‍❤‍💋‍👨🏼", "👩🏼‍❤️‍💋‍👨🏽", "👩🏼‍❤‍💋‍👨🏽", "👩🏼‍❤️‍💋‍👨🏾", "👩🏼‍❤‍💋‍👨🏾", "👩🏼‍❤️‍💋‍👨🏿", "👩🏼‍❤‍💋‍👨🏿", "👩🏽‍❤️‍💋‍👨🏻", "👩🏽‍❤‍💋‍👨🏻", "👩🏽‍❤️‍💋‍👨🏼", "👩🏽‍❤‍💋‍👨🏼", "👩🏽‍❤️‍💋‍👨🏽", "👩🏽‍❤‍💋‍👨🏽", "👩🏽‍❤️‍💋‍👨🏾", "👩🏽‍❤‍💋‍👨🏾", "👩🏽‍❤️‍💋‍👨🏿", "👩🏽‍❤‍💋‍👨🏿", "👩🏾‍❤️‍💋‍👨🏻", "👩🏾‍❤‍💋‍👨🏻", "👩🏾‍❤️‍💋‍👨🏼", "👩🏾‍❤‍💋‍👨🏼", "👩🏾‍❤️‍💋‍👨🏽", "👩🏾‍❤‍💋‍👨🏽", "👩🏾‍❤️‍💋‍👨🏾", "👩🏾‍❤‍💋‍👨🏾", "👩🏾‍❤️‍💋‍👨🏿", "👩🏾‍❤‍💋‍👨🏿", "👩🏿‍❤️‍💋‍👨🏻", "👩🏿‍❤‍💋‍👨🏻", "👩🏿‍❤️‍💋‍👨🏼", "👩🏿‍❤‍💋‍👨🏼", "👩🏿‍❤️‍💋‍👨🏽", "👩🏿‍❤‍💋‍👨🏽", "👩🏿‍❤️‍💋‍👨🏾", "👩🏿‍❤‍💋‍👨🏾", "👩🏿‍❤️‍💋‍👨🏿", "👩🏿‍❤‍💋‍👨🏿", "👨‍❤️‍💋‍👨", "👨‍❤‍💋‍👨", "👨🏻‍❤️‍💋‍👨🏻", "👨🏻‍❤‍💋‍👨🏻", "👨🏻‍❤️‍💋‍👨🏼", "👨🏻‍❤‍💋‍👨🏼", "👨🏻‍❤️‍💋‍👨🏽", "👨🏻‍❤‍💋‍👨🏽", "👨🏻‍❤️‍💋‍👨🏾", "👨🏻‍❤‍💋‍👨🏾", "👨🏻‍❤️‍💋‍👨🏿", "👨🏻‍❤‍💋‍👨🏿", "👨🏼‍❤️‍💋‍👨🏻", "👨🏼‍❤‍💋‍👨🏻", "👨🏼‍❤️‍💋‍👨🏼", "👨🏼‍❤‍💋‍👨🏼", "👨🏼‍❤️‍💋‍👨🏽", "👨🏼‍❤‍💋‍👨🏽", "👨🏼‍❤️‍💋‍👨🏾", "👨🏼‍❤‍💋‍👨🏾", "👨🏼‍❤️‍💋‍👨🏿", "👨🏼‍❤‍💋‍👨🏿", "👨🏽‍❤️‍💋‍👨🏻", "👨🏽‍❤‍💋‍👨🏻", "👨🏽‍❤️‍💋‍👨🏼", "👨🏽‍❤‍💋‍👨🏼", "👨🏽‍❤️‍💋‍👨🏽", "👨🏽‍❤‍💋‍👨🏽", "👨🏽‍❤️‍💋‍👨🏾", "👨🏽‍❤‍💋‍👨🏾", "👨🏽‍❤️‍💋‍👨🏿", "👨🏽‍❤‍💋‍👨🏿", "👨🏾‍❤️‍💋‍👨🏻", "👨🏾‍❤‍💋‍👨🏻", "👨🏾‍❤️‍💋‍👨🏼", "👨🏾‍❤‍💋‍👨🏼", "👨🏾‍❤️‍💋‍👨🏽", "👨🏾‍❤‍💋‍👨🏽", "👨🏾‍❤️‍💋‍👨🏾", "👨🏾‍❤‍💋‍👨🏾", "👨🏾‍❤️‍💋‍👨🏿", "👨🏾‍❤‍💋‍👨🏿", "👨🏿‍❤️‍💋‍👨🏻", "👨🏿‍❤‍💋‍👨🏻", "👨🏿‍❤️‍💋‍👨🏼", "👨🏿‍❤‍💋‍👨🏼", "👨🏿‍❤️‍💋‍👨🏽", "👨🏿‍❤‍💋‍👨🏽", "👨🏿‍❤️‍💋‍👨🏾", "👨🏿‍❤‍💋‍👨🏾", "👨🏿‍❤️‍💋‍👨🏿", "👨🏿‍❤‍💋‍👨🏿", "👩‍❤️‍💋‍👩", "👩‍❤‍💋‍👩", "👩🏻‍❤️‍💋‍👩🏻", "👩🏻‍❤‍💋‍👩🏻", "👩🏻‍❤️‍💋‍👩🏼", "👩🏻‍❤‍💋‍👩🏼", "👩🏻‍❤️‍💋‍👩🏽", "👩🏻‍❤‍💋‍👩🏽", "👩🏻‍❤️‍💋‍👩🏾", "👩🏻‍❤‍💋‍👩🏾", "👩🏻‍❤️‍💋‍👩🏿", "👩🏻‍❤‍💋‍👩🏿", "👩🏼‍❤️‍💋‍👩🏻", "👩🏼‍❤‍💋‍👩🏻", "👩🏼‍❤️‍💋‍👩🏼", "👩🏼‍❤‍💋‍👩🏼", "👩🏼‍❤️‍💋‍👩🏽", "👩🏼‍❤‍💋‍👩🏽", "👩🏼‍❤️‍💋‍👩🏾", "👩🏼‍❤‍💋‍👩🏾", "👩🏼‍❤️‍💋‍👩🏿", "👩🏼‍❤‍💋‍👩🏿", "👩🏽‍❤️‍💋‍👩🏻", "👩🏽‍❤‍💋‍👩🏻", "👩🏽‍❤️‍💋‍👩🏼", "👩🏽‍❤‍💋‍👩🏼", "👩🏽‍❤️‍💋‍👩🏽", "👩🏽‍❤‍💋‍👩🏽", "👩🏽‍❤️‍💋‍👩🏾", "👩🏽‍❤‍💋‍👩🏾", "👩🏽‍❤️‍💋‍👩🏿", "👩🏽‍❤‍💋‍👩🏿", "👩🏾‍❤️‍💋‍👩🏻", "👩🏾‍❤‍💋‍👩🏻", "👩🏾‍❤️‍💋‍👩🏼", "👩🏾‍❤‍💋‍👩🏼", "👩🏾‍❤️‍💋‍👩🏽", "👩🏾‍❤‍💋‍👩🏽", "👩🏾‍❤️‍💋‍👩🏾", "👩🏾‍❤‍💋‍👩🏾", "👩🏾‍❤️‍💋‍👩🏿", "👩🏾‍❤‍💋‍👩🏿", "👩🏿‍❤️‍💋‍👩🏻", "👩🏿‍❤‍💋‍👩🏻", "👩🏿‍❤️‍💋‍👩🏼", "👩🏿‍❤‍💋‍👩🏼", "👩🏿‍❤️‍💋‍👩🏽", "👩🏿‍❤‍💋‍👩🏽", "👩🏿‍❤️‍💋‍👩🏾", "👩🏿‍❤‍💋‍👩🏾", "👩🏿‍❤️‍💋‍👩🏿", "👩🏿‍❤‍💋‍👩🏿", "💑", "💑🏻", "💑🏼", "💑🏽", "💑🏾", "💑🏿", "🧑🏻‍❤️‍🧑🏼", "🧑🏻‍❤‍🧑🏼", "🧑🏻‍❤️‍🧑🏽", "🧑🏻‍❤‍🧑🏽", "🧑🏻‍❤️‍🧑🏾", "🧑🏻‍❤‍🧑🏾", "🧑🏻‍❤️‍🧑🏿", "🧑🏻‍❤‍🧑🏿", "🧑🏼‍❤️‍🧑🏻", "🧑🏼‍❤‍🧑🏻", "🧑🏼‍❤️‍🧑🏽", "🧑🏼‍❤‍🧑🏽", "🧑🏼‍❤️‍🧑🏾", "🧑🏼‍❤‍🧑🏾", "🧑🏼‍❤️‍🧑🏿", "🧑🏼‍❤‍🧑🏿", "🧑🏽‍❤️‍🧑🏻", "🧑🏽‍❤‍🧑🏻", "🧑🏽‍❤️‍🧑🏼", "🧑🏽‍❤‍🧑🏼", "🧑🏽‍❤️‍🧑🏾", "🧑🏽‍❤‍🧑🏾", "🧑🏽‍❤️‍🧑🏿", "🧑🏽‍❤‍🧑🏿", "🧑🏾‍❤️‍🧑🏻", "🧑🏾‍❤‍🧑🏻", "🧑🏾‍❤️‍🧑🏼", "🧑🏾‍❤‍🧑🏼", "🧑🏾‍❤️‍🧑🏽", "🧑🏾‍❤‍🧑🏽", "🧑🏾‍❤️‍🧑🏿", "🧑🏾‍❤‍🧑🏿", "🧑🏿‍❤️‍🧑🏻", "🧑🏿‍❤‍🧑🏻", "🧑🏿‍❤️‍🧑🏼", "🧑🏿‍❤‍🧑🏼", "🧑🏿‍❤️‍🧑🏽", "🧑🏿‍❤‍🧑🏽", "🧑🏿‍❤️‍🧑🏾", "🧑🏿‍❤‍🧑🏾", "👩‍❤️‍👨", "👩‍❤‍👨", "👩🏻‍❤️‍👨🏻", "👩🏻‍❤‍👨🏻", "👩🏻‍❤️‍👨🏼", "👩🏻‍❤‍👨🏼", "👩🏻‍❤️‍👨🏽", "👩🏻‍❤‍👨🏽", "👩🏻‍❤️‍👨🏾", "👩🏻‍❤‍👨🏾", "👩🏻‍❤️‍👨🏿", "👩🏻‍❤‍👨🏿", "👩🏼‍❤️‍👨🏻", "👩🏼‍❤‍👨🏻", "👩🏼‍❤️‍👨🏼", "👩🏼‍❤‍👨🏼", "👩🏼‍❤️‍👨🏽", "👩🏼‍❤‍👨🏽", "👩🏼‍❤️‍👨🏾", "👩🏼‍❤‍👨🏾", "👩🏼‍❤️‍👨🏿", "👩🏼‍❤‍👨🏿", "👩🏽‍❤️‍👨🏻", "👩🏽‍❤‍👨🏻", "👩🏽‍❤️‍👨🏼", "👩🏽‍❤‍👨🏼", "👩🏽‍❤️‍👨🏽", "👩🏽‍❤‍👨🏽", "👩🏽‍❤️‍👨🏾", "👩🏽‍❤‍👨🏾", "👩🏽‍❤️‍👨🏿", "👩🏽‍❤‍👨🏿", "👩🏾‍❤️‍👨🏻", "👩🏾‍❤‍👨🏻", "👩🏾‍❤️‍👨🏼", "👩🏾‍❤‍👨🏼", "👩🏾‍❤️‍👨🏽", "👩🏾‍❤‍👨🏽", "👩🏾‍❤️‍👨🏾", "👩🏾‍❤‍👨🏾", "👩🏾‍❤️‍👨🏿", "👩🏾‍❤‍👨🏿", "👩🏿‍❤️‍👨🏻", "👩🏿‍❤‍👨🏻", "👩🏿‍❤️‍👨🏼", "👩🏿‍❤‍👨🏼", "👩🏿‍❤️‍👨🏽", "👩🏿‍❤‍👨🏽", "👩🏿‍❤️‍👨🏾", "👩🏿‍❤‍👨🏾", "👩🏿‍❤️‍👨🏿", "👩🏿‍❤‍👨🏿", "👨‍❤️‍👨", "👨‍❤‍👨", "👨🏻‍❤️‍👨🏻", "👨🏻‍❤‍👨🏻", "👨🏻‍❤️‍👨🏼", "👨🏻‍❤‍👨🏼", "👨🏻‍❤️‍👨🏽", "👨🏻‍❤‍👨🏽", "👨🏻‍❤️‍👨🏾", "👨🏻‍❤‍👨🏾", "👨🏻‍❤️‍👨🏿", "👨🏻‍❤‍👨🏿", "👨🏼‍❤️‍👨🏻", "👨🏼‍❤‍👨🏻", "👨🏼‍❤️‍👨🏼", "👨🏼‍❤‍👨🏼", "👨🏼‍❤️‍👨🏽", "👨🏼‍❤‍👨🏽", "👨🏼‍❤️‍👨🏾", "👨🏼‍❤‍👨🏾", "👨🏼‍❤️‍👨🏿", "👨🏼‍❤‍👨🏿", "👨🏽‍❤️‍👨🏻", "👨🏽‍❤‍👨🏻", "👨🏽‍❤️‍👨🏼", "👨🏽‍❤‍👨🏼", "👨🏽‍❤️‍👨🏽", "👨🏽‍❤‍👨🏽", "👨🏽‍❤️‍👨🏾", "👨🏽‍❤‍👨🏾", "👨🏽‍❤️‍👨🏿", "👨🏽‍❤‍👨🏿", "👨🏾‍❤️‍👨🏻", "👨🏾‍❤‍👨🏻", "👨🏾‍❤️‍👨🏼", "👨🏾‍❤‍👨🏼", "👨🏾‍❤️‍👨🏽", "👨🏾‍❤‍👨🏽", "👨🏾‍❤️‍👨🏾", "👨🏾‍❤‍👨🏾", "👨🏾‍❤️‍👨🏿", "👨🏾‍❤‍👨🏿", "👨🏿‍❤️‍👨🏻", "👨🏿‍❤‍👨🏻", "👨🏿‍❤️‍👨🏼", "👨🏿‍❤‍👨🏼", "👨🏿‍❤️‍👨🏽", "👨🏿‍❤‍👨🏽", "👨🏿‍❤️‍👨🏾", "👨🏿‍❤‍👨🏾", "👨🏿‍❤️‍👨🏿", "👨🏿‍❤‍👨🏿", "👩‍❤️‍👩", "👩‍❤‍👩", "👩🏻‍❤️‍👩🏻", "👩🏻‍❤‍👩🏻", "👩🏻‍❤️‍👩🏼", "👩🏻‍❤‍👩🏼", "👩🏻‍❤️‍👩🏽", "👩🏻‍❤‍👩🏽", "👩🏻‍❤️‍👩🏾", "👩🏻‍❤‍👩🏾", "👩🏻‍❤️‍👩🏿", "👩🏻‍❤‍👩🏿", "👩🏼‍❤️‍👩🏻", "👩🏼‍❤‍👩🏻", "👩🏼‍❤️‍👩🏼", "👩🏼‍❤‍👩🏼", "👩🏼‍❤️‍👩🏽", "👩🏼‍❤‍👩🏽", "👩🏼‍❤️‍👩🏾", "👩🏼‍❤‍👩🏾", "👩🏼‍❤️‍👩🏿", "👩🏼‍❤‍👩🏿", "👩🏽‍❤️‍👩🏻", "👩🏽‍❤‍👩🏻", "👩🏽‍❤️‍👩🏼", "👩🏽‍❤‍👩🏼", "👩🏽‍❤️‍👩🏽", "👩🏽‍❤‍👩🏽", "👩🏽‍❤️‍👩🏾", "👩🏽‍❤‍👩🏾", "👩🏽‍❤️‍👩🏿", "👩🏽‍❤‍👩🏿", "👩🏾‍❤️‍👩🏻", "👩🏾‍❤‍👩🏻", "👩🏾‍❤️‍👩🏼", "👩🏾‍❤‍👩🏼", "👩🏾‍❤️‍👩🏽", "👩🏾‍❤‍👩🏽", "👩🏾‍❤️‍👩🏾", "👩🏾‍❤‍👩🏾", "👩🏾‍❤️‍👩🏿", "👩🏾‍❤‍👩🏿", "👩🏿‍❤️‍👩🏻", "👩🏿‍❤‍👩🏻", "👩🏿‍❤️‍👩🏼", "👩🏿‍❤‍👩🏼", "👩🏿‍❤️‍👩🏽", "👩🏿‍❤‍👩🏽", "👩🏿‍❤️‍👩🏾", "👩🏿‍❤‍👩🏾", "👩🏿‍❤️‍👩🏿", "👩🏿‍❤‍👩🏿", "👪", "👨‍👩‍👦", "👨‍👩‍👧", "👨‍👩‍👧‍👦", "👨‍👩‍👦‍👦", "👨‍👩‍👧‍👧", "👨‍👨‍👦", "👨‍👨‍👧", "👨‍👨‍👧‍👦", "👨‍👨‍👦‍👦", "👨‍👨‍👧‍👧", "👩‍👩‍👦", "👩‍👩‍👧", "👩‍👩‍👧‍👦", "👩‍👩‍👦‍👦", "👩‍👩‍👧‍👧", "👨‍👦", "👨‍👦‍👦", "👨‍👧", "👨‍👧‍👦", "👨‍👧‍👧", "👩‍👦", "👩‍👦‍👦", "👩‍👧", "👩‍👧‍👦", "👩‍👧‍👧", "🗣️", "🗣", "👤", "👥", "🫂", "👣", "🏻", "🏼", "🏽", "🏾", "🏿", "🦰", "🦱", "🦳", "🦲", "🐵", "🐒", "🦍", "🦧", "🐶", "🐕", "🦮", "🐕‍🦺", "🐩", "🐺", "🦊", "🦝", "🐱", "🐈", "🐈‍⬛", "🦁", "🐯", "🐅", "🐆", "🐴", "🐎", "🦄", "🦓", "🦌", "🦬", "🐮", "🐂", "🐃", "🐄", "🐷", "🐖", "🐗", "🐽", "🐏", "🐑", "🐐", "🐪", "🐫", "🦙", "🦒", "🐘", "🦣", "🦏", "🦛", "🐭", "🐁", "🐀", "🐹", "🐰", "🐇", "🐿️", "🐿", "🦫", "🦔", "🦇", "🐻", "🐻‍❄️", "🐻‍❄", "🐨", "🐼", "🦥", "🦦", "🦨", "🦘", "🦡", "🐾", "🦃", "🐔", "🐓", "🐣", "🐤", "🐥", "🐦", "🐧", "🕊️", "🕊", "🦅", "🦆", "🦢", "🦉", "🦤", "🪶", "🦩", "🦚", "🦜", "🐸", "🐊", "🐢", "🦎", "🐍", "🐲", "🐉", "🦕", "🦖", "🐳", "🐋", "🐬", "🦭", "🐟", "🐠", "🐡", "🦈", "🐙", "🐚", "🐌", "🦋", "🐛", "🐜", "🐝", "🪲", "🐞", "🦗", "🪳", "🕷️", "🕷", "🕸️", "🕸", "🦂", "🦟", "🪰", "🪱", "🦠", "💐", "🌸", "💮", "🏵️", "🏵", "🌹", "🥀", "🌺", "🌻", "🌼", "🌷", "🌱", "🪴", "🌲", "🌳", "🌴", "🌵", "🌾", "🌿", "☘️", "☘", "🍀", "🍁", "🍂", "🍃", "🍇", "🍈", "🍉", "🍊", "🍋", "🍌", "🍍", "🥭", "🍎", "🍏", "🍐", "🍑", "🍒", "🍓", "🫐", "🥝", "🍅", "🫒", "🥥", "🥑", "🍆", "🥔", "🥕", "🌽", "🌶️", "🌶", "🫑", "🥒", "🥬", "🥦", "🧄", "🧅", "🍄", "🥜", "🌰", "🍞", "🥐", "🥖", "🫓", "🥨", "🥯", "🥞", "🧇", "🧀", "🍖", "🍗", "🥩", "🥓", "🍔", "🍟", "🍕", "🌭", "🥪", "🌮", "🌯", "🫔", "🥙", "🧆", "🥚", "🍳", "🥘", "🍲", "🫕", "🥣", "🥗", "🍿", "🧈", "🧂", "🥫", "🍱", "🍘", "🍙", "🍚", "🍛", "🍜", "🍝", "🍠", "🍢", "🍣", "🍤", "🍥", "🥮", "🍡", "🥟", "🥠", "🥡", "🦀", "🦞", "🦐", "🦑", "🦪", "🍦", "🍧", "🍨", "🍩", "🍪", "🎂", "🍰", "🧁", "🥧", "🍫", "🍬", "🍭", "🍮", "🍯", "🍼", "🥛", "☕", "🫖", "🍵", "🍶", "🍾", "🍷", "🍸", "🍹", "🍺", "🍻", "🥂", "🥃", "🥤", "🧋", "🧃", "🧉", "🧊", "🥢", "🍽️", "🍽", "🍴", "🥄", "🔪", "🏺", "🌍", "🌎", "🌏", "🌐", "🗺️", "🗺", "🗾", "🧭", "🏔️", "🏔", "⛰️", "⛰", "🌋", "🗻", "🏕️", "🏕", "🏖️", "🏖", "🏜️", "🏜", "🏝️", "🏝", "🏞️", "🏞", "🏟️", "🏟", "🏛️", "🏛", "🏗️", "🏗", "🧱", "🪨", "🪵", "🛖", "🏘️", "🏘", "🏚️", "🏚", "🏠", "🏡", "🏢", "🏣", "🏤", "🏥", "🏦", "🏨", "🏩", "🏪", "🏫", "🏬", "🏭", "🏯", "🏰", "💒", "🗼", "🗽", "⛪", "🕌", "🛕", "🕍", "⛩️", "⛩", "🕋", "⛲", "⛺", "🌁", "🌃", "🏙️", "🏙", "🌄", "🌅", "🌆", "🌇", "🌉", "♨️", "♨", "🎠", "🎡", "🎢", "💈", "🎪", "🚂", "🚃", "🚄", "🚅", "🚆", "🚇", "🚈", "🚉", "🚊", "🚝", "🚞", "🚋", "🚌", "🚍", "🚎", "🚐", "🚑", "🚒", "🚓", "🚔", "🚕", "🚖", "🚗", "🚘", "🚙", "🛻", "🚚", "🚛", "🚜", "🏎️", "🏎", "🏍️", "🏍", "🛵", "🦽", "🦼", "🛺", "🚲", "🛴", "🛹", "🛼", "🚏", "🛣️", "🛣", "🛤️", "🛤", "🛢️", "🛢", "⛽", "🚨", "🚥", "🚦", "🛑", "🚧", "⚓", "⛵", "🛶", "🚤", "🛳️", "🛳", "⛴️", "⛴", "🛥️", "🛥", "🚢", "✈️", "✈", "🛩️", "🛩", "🛫", "🛬", "🪂", "💺", "🚁", "🚟", "🚠", "🚡", "🛰️", "🛰", "🚀", "🛸", "🛎️", "🛎", "🧳", "⌛", "⏳", "⌚", "⏰", "⏱️", "⏱", "⏲️", "⏲", "🕰️", "🕰", "🕛", "🕧", "🕐", "🕜", "🕑", "🕝", "🕒", "🕞", "🕓", "🕟", "🕔", "🕠", "🕕", "🕡", "🕖", "🕢", "🕗", "🕣", "🕘", "🕤", "🕙", "🕥", "🕚", "🕦", "🌑", "🌒", "🌓", "🌔", "🌕", "🌖", "🌗", "🌘", "🌙", "🌚", "🌛", "🌜", "🌡️", "🌡", "☀️", "☀", "🌝", "🌞", "🪐", "⭐", "🌟", "🌠", "🌌", "☁️", "☁", "⛅", "⛈️", "⛈", "🌤️", "🌤", "🌥️", "🌥", "🌦️", "🌦", "🌧️", "🌧", "🌨️", "🌨", "🌩️", "🌩", "🌪️", "🌪", "🌫️", "🌫", "🌬️", "🌬", "🌀", "🌈", "🌂", "☂️", "☂", "☔", "⛱️", "⛱", "⚡", "❄️", "❄", "☃️", "☃", "⛄", "☄️", "☄", "🔥", "💧", "🌊", "🎃", "🎄", "🎆", "🎇", "🧨", "✨", "🎈", "🎉", "🎊", "🎋", "🎍", "🎎", "🎏", "🎐", "🎑", "🧧", "🎀", "🎁", "🎗️", "🎗", "🎟️", "🎟", "🎫", "🎖️", "🎖", "🏆", "🏅", "🥇", "🥈", "🥉", "⚽", "⚾", "🥎", "🏀", "🏐", "🏈", "🏉", "🎾", "🥏", "🎳", "🏏", "🏑", "🏒", "🥍", "🏓", "🏸", "🥊", "🥋", "🥅", "⛳", "⛸️", "⛸", "🎣", "🤿", "🎽", "🎿", "🛷", "🥌", "🎯", "🪀", "🪁", "🎱", "🔮", "🪄", "🧿", "🎮", "🕹️", "🕹", "🎰", "🎲", "🧩", "🧸", "🪅", "🪆", "♠️", "♠", "♥️", "♥", "♦️", "♦", "♣️", "♣", "♟️", "♟", "🃏", "🀄", "🎴", "🎭", "🖼️", "🖼", "🎨", "🧵", "🪡", "🧶", "🪢", "👓", "🕶️", "🕶", "🥽", "🥼", "🦺", "👔", "👕", "👖", "🧣", "🧤", "🧥", "🧦", "👗", "👘", "🥻", "🩱", "🩲", "🩳", "👙", "👚", "👛", "👜", "👝", "🛍️", "🛍", "🎒", "🩴", "👞", "👟", "🥾", "🥿", "👠", "👡", "🩰", "👢", "👑", "👒", "🎩", "🎓", "🧢", "🪖", "⛑️", "⛑", "📿", "💄", "💍", "💎", "🔇", "🔈", "🔉", "🔊", "📢", "📣", "📯", "🔔", "🔕", "🎼", "🎵", "🎶", "🎙️", "🎙", "🎚️", "🎚", "🎛️", "🎛", "🎤", "🎧", "📻", "🎷", "🪗", "🎸", "🎹", "🎺", "🎻", "🪕", "🥁", "🪘", "📱", "📲", "☎️", "☎", "📞", "📟", "📠", "🔋", "🔌", "💻", "🖥️", "🖥", "🖨️", "🖨", "⌨️", "⌨", "🖱️", "🖱", "🖲️", "🖲", "💽", "💾", "💿", "📀", "🧮", "🎥", "🎞️", "🎞", "📽️", "📽", "🎬", "📺", "📷", "📸", "📹", "📼", "🔍", "🔎", "🕯️", "🕯", "💡", "🔦", "🏮", "🪔", "📔", "📕", "📖", "📗", "📘", "📙", "📚", "📓", "📒", "📃", "📜", "📄", "📰", "🗞️", "🗞", "📑", "🔖", "🏷️", "🏷", "💰", "🪙", "💴", "💵", "💶", "💷", "💸", "💳", "🧾", "💹", "✉️", "✉", "📧", "📨", "📩", "📤", "📥", "📦", "📫", "📪", "📬", "📭", "📮", "🗳️", "🗳", "✏️", "✏", "✒️", "✒", "🖋️", "🖋", "🖊️", "🖊", "🖌️", "🖌", "🖍️", "🖍", "📝", "💼", "📁", "📂", "🗂️", "🗂", "📅", "📆", "🗒️", "🗒", "🗓️", "🗓", "📇", "📈", "📉", "📊", "📋", "📌", "📍", "📎", "🖇️", "🖇", "📏", "📐", "✂️", "✂", "🗃️", "🗃", "🗄️", "🗄", "🗑️", "🗑", "🔒", "🔓", "🔏", "🔐", "🔑", "🗝️", "🗝", "🔨", "🪓", "⛏️", "⛏", "⚒️", "⚒", "🛠️", "🛠", "🗡️", "🗡", "⚔️", "⚔", "🔫", "🪃", "🏹", "🛡️", "🛡", "🪚", "🔧", "🪛", "🔩", "⚙️", "⚙", "🗜️", "🗜", "⚖️", "⚖", "🦯", "🔗", "⛓️", "⛓", "🪝", "🧰", "🧲", "🪜", "⚗️", "⚗", "🧪", "🧫", "🧬", "🔬", "🔭", "📡", "💉", "🩸", "💊", "🩹", "🩺", "🚪", "🛗", "🪞", "🪟", "🛏️", "🛏", "🛋️", "🛋", "🪑", "🚽", "🪠", "🚿", "🛁", "🪤", "🪒", "🧴", "🧷", "🧹", "🧺", "🧻", "🪣", "🧼", "🪥", "🧽", "🧯", "🛒", "🚬", "⚰️", "⚰", "🪦", "⚱️", "⚱", "🗿", "🪧", "🏧", "🚮", "🚰", "♿", "🚹", "🚺", "🚻", "🚼", "🚾", "🛂", "🛃", "🛄", "🛅", "⚠️", "⚠", "🚸", "⛔", "🚫", "🚳", "🚭", "🚯", "🚱", "🚷", "📵", "🔞", "☢️", "☢", "☣️", "☣", "⬆️", "⬆", "↗️", "↗", "➡️", "➡", "↘️", "↘", "⬇️", "⬇", "↙️", "↙", "⬅️", "⬅", "↖️", "↖", "↕️", "↕", "↔️", "↔", "↩️", "↩", "↪️", "↪", "⤴️", "⤴", "⤵️", "⤵", "🔃", "🔄", "🔙", "🔚", "🔛", "🔜", "🔝", "🛐", "⚛️", "⚛", "🕉️", "🕉", "✡️", "✡", "☸️", "☸", "☯️", "☯", "✝️", "✝", "☦️", "☦", "☪️", "☪", "☮️", "☮", "🕎", "🔯", "♈", "♉", "♊", "♋", "♌", "♍", "♎", "♏", "♐", "♑", "♒", "♓", "⛎", "🔀", "🔁", "🔂", "▶️", "▶", "⏩", "⏭️", "⏭", "⏯️", "⏯", "◀️", "◀", "⏪", "⏮️", "⏮", "🔼", "⏫", "🔽", "⏬", "⏸️", "⏸", "⏹️", "⏹", "⏺️", "⏺", "⏏️", "⏏", "🎦", "🔅", "🔆", "📶", "📳", "📴", "♀️", "♀", "♂️", "♂", "⚧️", "⚧", "✖️", "✖", "➕", "➖", "➗", "♾️", "♾", "‼️", "‼", "⁉️", "⁉", "❓", "❔", "❕", "❗", "〰️", "〰", "💱", "💲", "⚕️", "⚕", "♻️", "♻", "⚜️", "⚜", "🔱", "📛", "🔰", "⭕", "✅", "☑️", "☑", "✔️", "✔", "❌", "❎", "➰", "➿", "〽️", "〽", "✳️", "✳", "✴️", "✴", "❇️", "❇", "©️", "©", "®️", "®", "™️", "™", "#️⃣", "#⃣", "*️⃣", "*⃣", "0️⃣", "0⃣", "1️⃣", "1⃣", "2️⃣", "2⃣", "3️⃣", "3⃣", "4️⃣", "4⃣", "5️⃣", "5⃣", "6️⃣", "6⃣", "7️⃣", "7⃣", "8️⃣", "8⃣", "9️⃣", "9⃣", "🔟", "🔠", "🔡", "🔢", "🔣", "🔤", "🅰️", "🅰", "🆎", "🅱️", "🅱", "🆑", "🆒", "🆓", "ℹ️", "ℹ", "🆔", "Ⓜ️", "Ⓜ", "🆕", "🆖", "🅾️", "🅾", "🆗", "🅿️", "🅿", "🆘", "🆙", "🆚", "🈁", "🈂️", "🈂", "🈷️", "🈷", "🈶", "🈯", "🉐", "🈹", "🈚", "🈲", "🉑", "🈸", "🈴", "🈳", "㊗️", "㊗", "㊙️", "㊙", "🈺", "🈵", "🔴", "🟠", "🟡", "🟢", "🔵", "🟣", "🟤", "⚫", "⚪", "🟥", "🟧", "🟨", "🟩", "🟦", "🟪", "🟫", "⬛", "⬜", "◼️", "◼", "◻️", "◻", "◾", "◽", "▪️", "▪", "▫️", "▫", "🔶", "🔷", "🔸", "🔹", "🔺", "🔻", "💠", "🔘", "🔳", "🔲", "🏁", "🚩", "🎌", "🏴", "🏳️", "🏳", "🏳️‍🌈", "🏳‍🌈", "🏳️‍⚧️", "🏳‍⚧️", "🏳️‍⚧", "🏳‍⚧", "🏴‍☠️", "🏴‍☠", "🇦🇨", "🇦🇩", "🇦🇪", "🇦🇫", "🇦🇬", "🇦🇮", "🇦🇱", "🇦🇲", "🇦🇴", "🇦🇶", "🇦🇷", "🇦🇸", "🇦🇹", "🇦🇺", "🇦🇼", "🇦🇽", "🇦🇿", "🇧🇦", "🇧🇧", "🇧🇩", "🇧🇪", "🇧🇫", "🇧🇬", "🇧🇭", "🇧🇮", "🇧🇯", "🇧🇱", "🇧🇲", "🇧🇳", "🇧🇴", "🇧🇶", "🇧🇷", "🇧🇸", "🇧🇹", "🇧🇻", "🇧🇼", "🇧🇾", "🇧🇿", "🇨🇦", "🇨🇨", "🇨🇩", "🇨🇫", "🇨🇬", "🇨🇭", "🇨🇮", "🇨🇰", "🇨🇱", "🇨🇲", "🇨🇳", "🇨🇴", "🇨🇵", "🇨🇷", "🇨🇺", "🇨🇻", "🇨🇼", "🇨🇽", "🇨🇾", "🇨🇿", "🇩🇪", "🇩🇬", "🇩🇯", "🇩🇰", "🇩🇲", "🇩🇴", "🇩🇿", "🇪🇦", "🇪🇨", "🇪🇪", "🇪🇬", "🇪🇭", "🇪🇷", "🇪🇸", "🇪🇹", "🇪🇺", "🇫🇮", "🇫🇯", "🇫🇰", "🇫🇲", "🇫🇴", "🇫🇷", "🇬🇦", "🇬🇧", "🇬🇩", "🇬🇪", "🇬🇫", "🇬🇬", "🇬🇭", "🇬🇮", "🇬🇱", "🇬🇲", "🇬🇳", "🇬🇵", "🇬🇶", "🇬🇷", "🇬🇸", "🇬🇹", "🇬🇺", "🇬🇼", "🇬🇾", "🇭🇰", "🇭🇲", "🇭🇳", "🇭🇷", "🇭🇹", "🇭🇺", "🇮🇨", "🇮🇩", "🇮🇪", "🇮🇱", "🇮🇲", "🇮🇳", "🇮🇴", "🇮🇶", "🇮🇷", "🇮🇸", "🇮🇹", "🇯🇪", "🇯🇲", "🇯🇴", "🇯🇵", "🇰🇪", "🇰🇬", "🇰🇭", "🇰🇮", "🇰🇲", "🇰🇳", "🇰🇵", "🇰🇷", "🇰🇼", "🇰🇾", "🇰🇿", "🇱🇦", "🇱🇧", "🇱🇨", "🇱🇮", "🇱🇰", "🇱🇷", "🇱🇸", "🇱🇹", "🇱🇺", "🇱🇻", "🇱🇾", "🇲🇦", "🇲🇨", "🇲🇩", "🇲🇪", "🇲🇫", "🇲🇬", "🇲🇭", "🇲🇰", "🇲🇱", "🇲🇲", "🇲🇳", "🇲🇴", "🇲🇵", "🇲🇶", "🇲🇷", "🇲🇸", "🇲🇹", "🇲🇺", "🇲🇻", "🇲🇼", "🇲🇽", "🇲🇾", "🇲🇿", "🇳🇦", "🇳🇨", "🇳🇪", "🇳🇫", "🇳🇬", "🇳🇮", "🇳🇱", "🇳🇴", "🇳🇵", "🇳🇷", "🇳🇺", "🇳🇿", "🇴🇲", "🇵🇦", "🇵🇪", "🇵🇫", "🇵🇬", "🇵🇭", "🇵🇰", "🇵🇱", "🇵🇲", "🇵🇳", "🇵🇷", "🇵🇸", "🇵🇹", "🇵🇼", "🇵🇾", "🇶🇦", "🇷🇪", "🇷🇴", "🇷🇸", "🇷🇺", "🇷🇼", "🇸🇦", "🇸🇧", "🇸🇨", "🇸🇩", "🇸🇪", "🇸🇬", "🇸🇭", "🇸🇮", "🇸🇯", "🇸🇰", "🇸🇱", "🇸🇲", "🇸🇳", "🇸🇴", "🇸🇷", "🇸🇸", "🇸🇹", "🇸🇻", "🇸🇽", "🇸🇾", "🇸🇿", "🇹🇦", "🇹🇨", "🇹🇩", "🇹🇫", "🇹🇬", "🇹🇭", "🇹🇯", "🇹🇰", "🇹🇱", "🇹🇲", "🇹🇳", "🇹🇴", "🇹🇷", "🇹🇹", "🇹🇻", "🇹🇼", "🇹🇿", "🇺🇦", "🇺🇬", "🇺🇲", "🇺🇳", "🇺🇸", "🇺🇾", "🇺🇿", "🇻🇦", "🇻🇨", "🇻🇪", "🇻🇬", "🇻🇮", "🇻🇳", "🇻🇺", "🇼🇫", "🇼🇸", "🇽🇰", "🇾🇪", "🇾🇹", "🇿🇦", "🇿🇲", "🇿🇼", "🏴󠁧󠁢󠁥󠁮󠁧󠁿", "🏴󠁧󠁢󠁳󠁣󠁴󠁿", "🏴󠁧󠁢󠁷󠁬󠁳󠁿" };
    }
}
