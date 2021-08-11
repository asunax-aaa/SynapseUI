define("ace/mode/lua_highlight_rules",["require","exports","module","ace/lib/oop","ace/mode/text_highlight_rules"], function(require, exports, module) {
    "use strict";
    
    var oop = require("../lib/oop");
    var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;
    
    var LuaHighlightRules = function() {
    
        var keywords = (
            "break|do|else|elseif|end|for|function|if|in|local|repeat|"+
             "return|then|until|while|or|and|not"
        );
    
        var builtinConstants = ("true|false|nil|_G|_VERSION");
    
        var functions = (
            "string|xpcall|package|tostring|print|os|unpack|require|"+
            "getfenv|setmetatable|next|assert|tonumber|io|rawequal|"+
            "collectgarbage|getmetatable|module|rawset|math|debug|"+
            "pcall|table|newproxy|type|coroutine|_G|select|gcinfo|"+
            "pairs|rawget|loadstring|ipairs|_VERSION|dofile|setfenv|"+
            "load|error|loadfile|"+
    
            "sub|upper|len|gfind|rep|find|match|char|dump|gmatch|"+
            "reverse|byte|format|gsub|lower|preload|loadlib|loaded|"+
            "loaders|cpath|config|path|seeall|exit|setlocale|date|"+
            "getenv|difftime|remove|time|clock|tmpname|rename|execute|"+
            "lines|write|close|flush|open|output|type|read|stderr|"+
            "stdin|input|stdout|popen|tmpfile|log|max|acos|huge|"+
            "ldexp|pi|cos|tanh|pow|deg|tan|cosh|sinh|random|randomseed|"+
            "frexp|ceil|floor|rad|abs|sqrt|modf|asin|min|mod|fmod|log10|"+
            "atan2|exp|sin|atan|getupvalue|debug|sethook|getmetatable|"+
            "gethook|setmetatable|setlocal|traceback|setfenv|getinfo|"+
            "setupvalue|getlocal|getregistry|getfenv|setn|insert|getn|"+
            "foreachi|maxn|foreach|concat|sort|remove|resume|yield|"+
            "status|wrap|create|running|"+
            "__add|__sub|__mod|__unm|__concat|__lt|__index|__call|__gc|__metatable|"+
            "__mul|__div|__pow|__len|__eq|__le|__newindex|__tostring|__mode|__tonumber"+

            "syn_websocket_close|firesignal|makefolder|syn_io_append|is_protosmasher_caller|clonefunction|setrawmetatable|syn_mouse2press|debug|syn_io_delfolder|getrawmetatable|getinstancefromstate|syn_io_makefolder|gethiddenprop|setfflag|gethiddenprops|getcallingscript|sethiddenprop|getrenv|syn_crypt_b64_encode|get_instances|newcclosure|gethiddenproperties|getspecialinfo|isluau|shared|decompile|loadstring|getprotos|syn_io_isfolder|hookfunction|isfile|getproto|print|isrbxactive|rconsoleinfo|make_readonly|getstack|rconsolename|unlockmodulescript|getupvalue|syn_getgc|syn_mouse2release|setproto|mouse1click|syn_io_read|setupvalue|syn_io_delfile|gethiddenproperty|identifyexecutor|getscripts|rconsoleerr|dumpstring|keypress|syn_mousescroll|syn_getinstances|syn_mouse1click|get_scripts|rconsoleclear|getlocals|is_redirection_enabled|syn_context_set|syn_keyrelease|syn_io_listdir|isreadonly|rconsoleprint|mouse2click|getinfo|sethiddenproperty|writefile|warn|loadfile|getproperties|getconstant|getprops|syn_setfflag|require|setscriptable|get_nil_instances|getnilinstances|is_synapse_function|getscriptclosure|bit|getconnections|checkcaller|syn|setclipboard|getupvalues|hookfunc|setsimulationradius|setreadonly|firetouchinterest|syn_getsenv|syn_io_isfile|syn_crypt_encrypt|getstates|mouse2press|syn_mouse1press|setconstant|validfgwindow|saveinstance|getinstances|getconstants|getloadedmodules|getgenv|syn_keypress|_G|messagebox|isnetworkowner|Drawing|delfile|mouse1release|get_loaded_modules|setnamecallmethod|syn_getreg|syn_dumpstring|syn_mousemoverel|syn_mouse1release|syn_getloadedmodules|syn_crypt_random|get_calling_script|XPROTECT|delfolder|syn_getcallingscript|keyrelease|appendfile|syn_islclosure|isfolder|listfiles|readfile|syn_websocket_connect|getcallstack|mousescroll|syn_crypt_hash|mousemoveabs|is_protosmasher_closure|syn_checkcaller|syn_mouse2click|mousemoverel|replaceclosure|mouse2release|getpcdprop|islclosure|rconsolewarn|getstateenv|syn_clipboard_set|syn_crypt_decrypt|readbinarystring|syn_getmenv|syn_crypt_b64_decode|mouse1press|syn_getrenv|syn_newcclosure|getpropvalue|syn_crypt_derive|syn_getgenv|getnamecallmethod|getgc|is_lclosure|getpointerfromstate|syn_decompile|setfpscap|getsenv|syn_mousemoveabs|setpropvalue|rconsoleinputasync|getlocal|make_writeable|fireclickdetector|printconsole|rconsoleinput|getmenv|getreg|syn_io_write|setlocal|messageboxasync|setstack|iswindowactive|syn_websocket_send|syn_context_get|syn_isactive|continue|" +
            "hookmetamethod|request|syn_websocket_close|firesignal|isuntouched|" +
            "getscripthash|setuntouched|getsynasset|fireproximityprompt|cloneref|" +
            "crypt|secrun|is_beta|secure_call|cache_replace|get_thread_identity|" +
            "request|protect_gui|run_secure_lua|cache_invalidate|queue_on_teleport|" +
            "is_cached|set_thread_identity|crypto|run_secure_function|websocket|" +
            "unprotect_gui|write_clipboard|" +

            "syn.crypt.encrypt|syn.crypt.custom.encrypt|syn.crypt.custom.hash|syn.crypt.custom.decrypt|syn.crypt.custom|syn.crypt.random|syn.crypt.decrypt|syn.crypt.hash|syn.crypt.derive|syn.crypt.base64.encode|syn.crypt.base64.decode|syn.crypt.base64|syn.crypt|syn.is_beta|syn.secure_call|syn.cache_replace|syn.get_thread_identity|syn.request|syn.protect_gui|syn.cache_invalidate|syn.queue_on_teleport|syn.is_cached|syn.set_thread_identity|syn.crypto.encrypt|syn.crypto.custom.encrypt|syn.crypto.custom.hash|syn.crypto.custom.decrypt|syn.crypto.custom|syn.crypto.random|syn.crypto.decrypt|syn.crypto.hash|syn.crypto.derive|syn.crypto.base64.encode|syn.crypto.base64.decode|syn.crypto.base64|syn.crypto|syn.create_secure_function|syn.run_secure_function|syn.websocket.connect|syn.websocket|syn.unprotect_gui|syn.write_clipboard|syn.crypt.lz4.compress|syn.crypt.lz4|syn.secrun|syn.run_secure_lua|syn.crypto.lz4.compress|syn.crypto.lz4"
        );
    
        var stdLibaries = ("string|package|os|io|math|debug|table|coroutine");
    
        var deprecatedIn5152 = ("setn|foreach|foreachi|gcinfo|log10|maxn");
    
        var keywordMapper = this.createKeywordMapper({
            "keyword": keywords,
            "support.function": functions,
            "keyword.deprecated": deprecatedIn5152,
            "constant.library": stdLibaries,
            "constant.language": builtinConstants,
            "variable.language": "self"
        }, "identifier");
    
        var decimalInteger = "(?:(?:[1-9]\\d*)|(?:0))";
        var hexInteger = "(?:0[xX][\\dA-Fa-f]+)";
        var integer = "(?:" + decimalInteger + "|" + hexInteger + ")";
    
        var fraction = "(?:\\.\\d+)";
        var intPart = "(?:\\d+)";
        var pointFloat = "(?:(?:" + intPart + "?" + fraction + ")|(?:" + intPart + "\\.))";
        var floatNumber = "(?:" + pointFloat + ")";
    
        this.$rules = {
            "start" : [{
                stateName: "bracketedComment",
                onMatch : function(value, currentState, stack){
                    stack.unshift(this.next, value.length - 2, currentState);
                    return "comment";
                },
                regex : /\-\-\[=*\[/,
                next  : [
                    {
                        onMatch : function(value, currentState, stack) {
                            if (value.length == stack[1]) {
                                stack.shift();
                                stack.shift();
                                this.next = stack.shift();
                            } else {
                                this.next = "";
                            }
                            return "comment";
                        },
                        regex : /\]=*\]/,
                        next  : "start"
                    }, {
                        defaultToken : "comment"
                    }
                ]
            },
    
            {
                token : "comment",
                regex : "\\-\\-.*$"
            },
            {
                stateName: "bracketedString",
                onMatch : function(value, currentState, stack){
                    stack.unshift(this.next, value.length, currentState);
                    return "string.start";
                },
                regex : /\[=*\[/,
                next  : [
                    {
                        onMatch : function(value, currentState, stack) {
                            if (value.length == stack[1]) {
                                stack.shift();
                                stack.shift();
                                this.next = stack.shift();
                            } else {
                                this.next = "";
                            }
                            return "string.end";
                        },
                        
                        regex : /\]=*\]/,
                        next  : "start"
                    }, {
                        defaultToken : "string"
                    }
                ]
            },
            {
                token : "string",           // " string
                regex : '"(?:[^\\\\]|\\\\.)*?"'
            }, {
                token : "string",           // ' string
                regex : "'(?:[^\\\\]|\\\\.)*?'"
            }, {
                token : "constant.numeric", // float
                regex : floatNumber
            }, {
                token : "constant.numeric", // integer
                regex : integer + "\\b"
            }, {
                token : keywordMapper,
                regex : "[a-zA-Z_$][a-zA-Z0-9_$]*\\b"
            }, {
                token : "keyword.operator",
                regex : "\\+|\\-|\\*|\\/|%|\\#|\\^|~|<|>|<=|=>|==|~=|=|\\:|\\.\\.\\.|\\.\\."
            }, {
                token : "paren.lparen",
                regex : "[\\[\\(\\{]"
            }, {
                token : "paren.rparen",
                regex : "[\\]\\)\\}]"
            }, {
                token : "text",
                regex : "\\s+|\\w+"
            } ]
        };
        
        this.normalizeRules();
    };
    
    oop.inherits(LuaHighlightRules, TextHighlightRules);
    
    exports.LuaHighlightRules = LuaHighlightRules;
    });
    
    define("ace/mode/folding/lua",["require","exports","module","ace/lib/oop","ace/mode/folding/fold_mode","ace/range","ace/token_iterator"], function(require, exports, module) {
    "use strict";
    
    var oop = require("../../lib/oop");
    var BaseFoldMode = require("./fold_mode").FoldMode;
    var Range = require("../../range").Range;
    var TokenIterator = require("../../token_iterator").TokenIterator;
    
    
    var FoldMode = exports.FoldMode = function() {};
    
    oop.inherits(FoldMode, BaseFoldMode);
    
    (function() {
    
        this.foldingStartMarker = /\b(function|then|do|repeat)\b|{\s*$|(\[=*\[)/;
        this.foldingStopMarker = /\bend\b|^\s*}|\]=*\]/;
    
        this.getFoldWidget = function(session, foldStyle, row) {
            var line = session.getLine(row);
            var isStart = this.foldingStartMarker.test(line);
            var isEnd = this.foldingStopMarker.test(line);
    
            if (isStart && !isEnd) {
                var match = line.match(this.foldingStartMarker);
                if (match[1] == "then" && /\belseif\b/.test(line))
                    return;
                if (match[1]) {
                    if (session.getTokenAt(row, match.index + 1).type === "keyword")
                        return "start";
                } else if (match[2]) {
                    var type = session.bgTokenizer.getState(row) || "";
                    if (type[0] == "bracketedComment" || type[0] == "bracketedString")
                        return "start";
                } else {
                    return "start";
                }
            }
            if (foldStyle != "markbeginend" || !isEnd || isStart && isEnd)
                return "";
    
            var match = line.match(this.foldingStopMarker);
            if (match[0] === "end") {
                if (session.getTokenAt(row, match.index + 1).type === "keyword")
                    return "end";
            } else if (match[0][0] === "]") {
                var type = session.bgTokenizer.getState(row - 1) || "";
                if (type[0] == "bracketedComment" || type[0] == "bracketedString")
                    return "end";
            } else
                return "end";
        };
    
        this.getFoldWidgetRange = function(session, foldStyle, row) {
            var line = session.doc.getLine(row);
            var match = this.foldingStartMarker.exec(line);
            if (match) {
                if (match[1])
                    return this.luaBlock(session, row, match.index + 1);
    
                if (match[2])
                    return session.getCommentFoldRange(row, match.index + 1);
    
                return this.openingBracketBlock(session, "{", row, match.index);
            }
    
            var match = this.foldingStopMarker.exec(line);
            if (match) {
                if (match[0] === "end") {
                    if (session.getTokenAt(row, match.index + 1).type === "keyword")
                        return this.luaBlock(session, row, match.index + 1);
                }
    
                if (match[0][0] === "]")
                    return session.getCommentFoldRange(row, match.index + 1);
    
                return this.closingBracketBlock(session, "}", row, match.index + match[0].length);
            }
        };
    
        this.luaBlock = function(session, row, column, tokenRange) {
            var stream = new TokenIterator(session, row, column);
            var indentKeywords = {
                "function": 1,
                "do": 1,
                "then": 1,
                "elseif": -1,
                "end": -1,
                "repeat": 1,
                "until": -1
            };
    
            var token = stream.getCurrentToken();
            if (!token || token.type != "keyword")
                return;
    
            var val = token.value;
            var stack = [val];
            var dir = indentKeywords[val];
    
            if (!dir)
                return;
    
            var startColumn = dir === -1 ? stream.getCurrentTokenColumn() : session.getLine(row).length;
            var startRow = row;
    
            stream.step = dir === -1 ? stream.stepBackward : stream.stepForward;
            while(token = stream.step()) {
                if (token.type !== "keyword")
                    continue;
                var level = dir * indentKeywords[token.value];
    
                if (level > 0) {
                    stack.unshift(token.value);
                } else if (level <= 0) {
                    stack.shift();
                    if (!stack.length && token.value != "elseif")
                        break;
                    if (level === 0)
                        stack.unshift(token.value);
                }
            }
    
            if (!token)
                return null;
    
            if (tokenRange)
                return stream.getCurrentTokenRange();
    
            var row = stream.getCurrentTokenRow();
            if (dir === -1)
                return new Range(row, session.getLine(row).length, startRow, startColumn);
            else
                return new Range(startRow, startColumn, row, stream.getCurrentTokenColumn());
        };
    
    }).call(FoldMode.prototype);
    
    });
    
    define("ace/mode/lua",["require","exports","module","ace/lib/oop","ace/mode/text","ace/mode/lua_highlight_rules","ace/mode/folding/lua","ace/range","ace/worker/worker_client"], function(require, exports, module) {
    "use strict";
    
    var oop = require("../lib/oop");
    var TextMode = require("./text").Mode;
    var LuaHighlightRules = require("./lua_highlight_rules").LuaHighlightRules;
    var LuaFoldMode = require("./folding/lua").FoldMode;
    var Range = require("../range").Range;
    var WorkerClient = require("../worker/worker_client").WorkerClient;
    
    var Mode = function() {
        this.HighlightRules = LuaHighlightRules;
        
        this.foldingRules = new LuaFoldMode();
        this.$behaviour = this.$defaultBehaviour;
    };
    oop.inherits(Mode, TextMode);
    
    (function() {
       
        this.lineCommentStart = "--";
        this.blockComment = {start: "--[", end: "]--"};
        
        var indentKeywords = {
            "function": 1,
            "then": 1,
            "do": 1,
            "else": 1,
            "elseif": 1,
            "repeat": 1,
            "end": -1,
            "until": -1
        };
        var outdentKeywords = [
            "else",
            "elseif",
            "end",
            "until"
        ];
    
        function getNetIndentLevel(tokens) {
            var level = 0;
            for (var i = 0; i < tokens.length; i++) {
                var token = tokens[i];
                if (token.type == "keyword") {
                    if (token.value in indentKeywords) {
                        level += indentKeywords[token.value];
                    }
                } else if (token.type == "paren.lparen") {
                    level += token.value.length;
                } else if (token.type == "paren.rparen") {
                    level -= token.value.length;
                }
            }
            if (level < 0) {
                return -1;
            } else if (level > 0) {
                return 1;
            } else {
                return 0;
            }
        }
    
        this.getNextLineIndent = function(state, line, tab) {
            var indent = this.$getIndent(line);
            var level = 0;
    
            var tokenizedLine = this.getTokenizer().getLineTokens(line, state);
            var tokens = tokenizedLine.tokens;
    
            if (state == "start") {
                level = getNetIndentLevel(tokens);
            }
            if (level > 0) {
                return indent + tab;
            } else if (level < 0 && indent.substr(indent.length - tab.length) == tab) {
                if (!this.checkOutdent(state, line, "\n")) {
                    return indent.substr(0, indent.length - tab.length);
                }
            }
            return indent;
        };
    
        this.checkOutdent = function(state, line, input) {
            if (input != "\n" && input != "\r" && input != "\r\n")
                return false;
    
            if (line.match(/^\s*[\)\}\]]$/))
                return true;
    
            var tokens = this.getTokenizer().getLineTokens(line.trim(), state).tokens;
    
            if (!tokens || !tokens.length)
                return false;
    
            return (tokens[0].type == "keyword" && outdentKeywords.indexOf(tokens[0].value) != -1);
        };
    
        this.getMatching = function(session, row, column) {
            if (row == undefined) {
                var pos = session.selection.lead;
                column = pos.column;
                row = pos.row;
            }
    
            var startToken = session.getTokenAt(row, column);
            if (startToken && startToken.value in indentKeywords)
                return this.foldingRules.luaBlock(session, row, column, true);
        };
    
        this.autoOutdent = function(state, session, row) {
            var line = session.getLine(row);
            var column = line.match(/^\s*/)[0].length;
            if (!column || !row) return;
    
            var startRange = this.getMatching(session, row, column + 1);
            if (!startRange || startRange.start.row == row)
                 return;
            var indent = this.$getIndent(session.getLine(startRange.start.row));
            if (indent.length != column) {
                session.replace(new Range(row, 0, row, column), indent);
                session.outdentRows(new Range(row + 1, 0, row + 1, 0));
            }
        };
    
        this.createWorker = function(session) {
            var worker = new WorkerClient(["ace"], "ace/mode/lua_worker", "Worker");
            worker.attachToDocument(session.getDocument());
            
            worker.on("annotate", function(e) {
                session.setAnnotations(e.data);
            });
            
            worker.on("terminate", function() {
                session.clearAnnotations();
            });
            
            return worker;
        };
    
        this.$id = "ace/mode/lua";
        this.snippetFileId = "ace/snippets/lua";
    }).call(Mode.prototype);
    
    exports.Mode = Mode;
    });                (function() {
                        window.require(["ace/mode/lua"], function(m) {
                            if (typeof module == "object" && typeof exports == "object" && module) {
                                module.exports = m;
                            }
                        });
                    })();
                