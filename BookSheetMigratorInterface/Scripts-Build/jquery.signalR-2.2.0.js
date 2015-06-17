/*!
 * ASP.NET SignalR JavaScript Library v2.2.0
 * http://signalr.net/
 *
 * Copyright (C) Microsoft Corporation. All rights reserved.
 *
 */

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

(function(e,t,n){function m(t,n){if(e.isArray(t)){for(var r=t.length-1;r>=0;r--){var s=t[r];if(e.type(s)!=="string"||!i.transports[s])n.log("Invalid transport: "+s+", removing it from the transports list."),t.splice(r,1)}t.length===0&&(n.log("No transports remain within the specified transport array."),t=null)}else if(!i.transports[t]&&t!=="auto")n.log("Invalid transport: "+t.toString()+"."),t=null;else if(t==="auto"&&i._.ieVersion<=8)return["longPolling"];return t}function g(e){if(e==="http:")return 80;if(e==="https:")return 443}function y(e,t){return t.match(/:\d+$/)?t:t+":"+g(e)}function b(t,n){var r=this,i=[];r.tryBuffer=function(n){return t.state===e.signalR.connectionState.connecting?(i.push(n),!0):!1},r.drain=function(){if(t.state===e.signalR.connectionState.connected)while(i.length>0)n(i.shift())},r.clear=function(){i=[]}}var r={nojQuery:"jQuery was not found. Please ensure jQuery is referenced before the SignalR client JavaScript file.",noTransportOnInit:"No transport could be initialized successfully. Try specifying a different transport or none at all for auto initialization.",errorOnNegotiate:"Error during negotiation request.",stoppedWhileLoading:"The connection was stopped during page load.",stoppedWhileNegotiating:"The connection was stopped during the negotiate request.",errorParsingNegotiateResponse:"Error parsing negotiate response.",errorDuringStartRequest:"Error during start request. Stopping the connection.",stoppedDuringStartRequest:"The connection was stopped during the start request.",errorParsingStartResponse:"Error parsing start response: '{0}'. Stopping the connection.",invalidStartResponse:"Invalid start response: '{0}'. Stopping the connection.",protocolIncompatible:"You are using a version of the client that isn't compatible with the server. Client version {0}, server version {1}.",sendFailed:"Send failed.",parseFailed:"Failed at parsing response: {0}",longPollFailed:"Long polling request failed.",eventSourceFailedToConnect:"EventSource failed to connect.",eventSourceError:"Error raised by EventSource",webSocketClosed:"WebSocket closed.",pingServerFailedInvalidResponse:"Invalid ping response when pinging server: '{0}'.",pingServerFailed:"Failed to ping server.",pingServerFailedStatusCode:"Failed to ping server.  Server responded with status code {0}, stopping the connection.",pingServerFailedParse:"Failed to parse ping server response, stopping the connection.",noConnectionTransport:"Connection is in an invalid state, there is no transport active.",webSocketsInvalidState:"The Web Socket transport is in an invalid state, transitioning into reconnecting.",reconnectTimeout:"Couldn't reconnect within the configured timeout of {0} ms, disconnecting.",reconnectWindowTimeout:"The client has been inactive since {0} and it has exceeded the inactivity timeout of {1} ms. Stopping the connection."};if(typeof e!="function")throw new Error(r.nojQuery);var i,s,o=t.document.readyState==="complete",u=e(t),a="__Negotiate Aborted__",f={onStart:"onStart",onStarting:"onStarting",onReceived:"onReceived",onError:"onError",onConnectionSlow:"onConnectionSlow",onReconnecting:"onReconnecting",onReconnect:"onReconnect",onStateChanged:"onStateChanged",onDisconnect:"onDisconnect"},l={processData:!0,timeout:null,async:!0,global:!1,cache:!1},c=function(e,n){if(n===!1)return;var r;if(typeof t.console=="undefined")return;r="["+(new Date).toTimeString()+"] SignalR: "+e,t.console.debug?t.console.debug(r):t.console.log&&t.console.log(r)},h=function(t,n,r){return n===t.state?(t.state=r,e(t).triggerHandler(f.onStateChanged,[{oldState:n,newState:r}]),!0):!1},p=function(e){return e.state===i.connectionState.disconnected},d=function(e){return e._.keepAliveData.activated&&e.transport.supportsKeepAlive(e)},v=function(n){var r,s;n._.configuredStopReconnectingTimeout||(s=function(t){var n=i._.format(i.resources.reconnectTimeout,t.disconnectTimeout);t.log(n),e(t).triggerHandler(f.onError,[i._.error(n,"TimeoutException")]),t.stop(!1,!1)},n.reconnecting(function(){var e=this;e.state===i.connectionState.reconnecting&&(r=t.setTimeout(function(){s(e)},e.disconnectTimeout))}),n.stateChanged(function(e){e.oldState===i.connectionState.reconnecting&&t.clearTimeout(r)}),n._.configuredStopReconnectingTimeout=!0)};i=function(e,t,n){return new i.fn.init(e,t,n)},i._={defaultContentType:"application/x-www-form-urlencoded; charset=UTF-8",ieVersion:function(){var e,n;return t.navigator.appName==="Microsoft Internet Explorer"&&(n=/MSIE ([0-9]+\.[0-9]+)/.exec(t.navigator.userAgent),n&&(e=t.parseFloat(n[1]))),e}(),error:function(e,t,n){var r=new Error(e);return r.source=t,typeof n!="undefined"&&(r.context=n),r},transportError:function(e,t,r,i){var s=this.error(e,r,i);return s.transport=t?t.name:n,s},format:function(){var e=arguments[0];for(var t=0;t<arguments.length-1;t++)e=e.replace("{"+t+"}",arguments[t+1]);return e},firefoxMajorVersion:function(e){var t=e.match(/Firefox\/(\d+)/);return!t||!t.length||t.length<2?0:parseInt(t[1],10)},configurePingInterval:function(n){var r=n._.config,s=function(t){e(n).triggerHandler(f.onError,[t])};r&&!n._.pingIntervalId&&r.pingInterval&&(n._.pingIntervalId=t.setInterval(function(){i.transports._logic.pingServer(n).fail(s)},r.pingInterval))}},i.events=f,i.resources=r,i.ajaxDefaults=l,i.changeState=h,i.isDisconnecting=p,i.connectionState={connecting:0,connected:1,reconnecting:2,disconnected:4},i.hub={start:function(){throw new Error("SignalR: Error loading hubs. Ensure your hubs reference is correct, e.g. <script src='/signalr/js'></script>.")}},u.load(function(){o=!0}),i.fn=i.prototype={init:function(t,n,r){var i=e(this);this.url=t,this.qs=n,this.lastError=null,this._={keepAliveData:{},connectingMessageBuffer:new b(this,function(e){i.triggerHandler(f.onReceived,[e])}),lastMessageAt:(new Date).getTime(),lastActiveAt:(new Date).getTime(),beatInterval:5e3,beatHandle:null,totalTransportConnectTimeout:0},typeof r=="boolean"&&(this.logging=r)},_parseResponse:function(e){var t=this;return e?typeof e=="string"?t.json.parse(e):e:e},_originalJson:t.JSON,json:t.JSON,isCrossDomain:function(n,r){var i;return n=e.trim(n),r=r||t.location,n.indexOf("http")!==0?!1:(i=t.document.createElement("a"),i.href=n,i.protocol+y(i.protocol,i.host)!==r.protocol+y(r.protocol,r.host))},ajaxDataType:"text",contentType:"application/json; charset=UTF-8",logging:!1,state:i.connectionState.disconnected,clientProtocol:"1.5",reconnectDelay:2e3,transportConnectTimeout:0,disconnectTimeout:3e4,reconnectWindow:3e4,keepAliveWarnAt:2/3,start:function(n,s){var l=this,c={pingInterval:3e5,waitForPageLoad:!0,transport:"auto",jsonp:!1},p,g=l._deferral||e.Deferred(),y=t.document.createElement("a");l.lastError=null,l._deferral=g;if(!l.json)throw new Error("SignalR: No JSON parser found. Please ensure json2.js is referenced before the SignalR.js file if you need to support clients without native JSON parsing support, e.g. IE<8.");e.type(n)==="function"?s=n:e.type(n)==="object"&&(e.extend(c,n),e.type(c.callback)==="function"&&(s=c.callback)),c.transport=m(c.transport,l);if(!c.transport)throw new Error("SignalR: Invalid transport(s) specified, aborting start.");l._.config=c;if(!o&&c.waitForPageLoad===!0)return l._.deferredStartHandler=function(){l.start(n,s)},u.bind("load",l._.deferredStartHandler),g.promise();if(l.state===i.connectionState.connecting)return g.promise();if(h(l,i.connectionState.disconnected,i.connectionState.connecting)===!1)return g.resolve(l),g.promise();v(l),y.href=l.url,!y.protocol||y.protocol===":"?(l.protocol=t.document.location.protocol,l.host=y.host||t.document.location.host):(l.protocol=y.protocol,l.host=y.host),l.baseUrl=l.protocol+"//"+l.host,l.wsProtocol=l.protocol==="https:"?"wss://":"ws://",c.transport==="auto"&&c.jsonp===!0&&(c.transport="longPolling"),l.url.indexOf("//")===0&&(l.url=t.location.protocol+l.url,l.log("Protocol relative URL detected, normalizing it to '"+l.url+"'.")),this.isCrossDomain(l.url)&&(l.log("Auto detected cross domain url."),c.transport==="auto"&&(c.transport=["webSockets","serverSentEvents","longPolling"]),typeof c.withCredentials=="undefined"&&(c.withCredentials=!0),c.jsonp||(c.jsonp=!e.support.cors,c.jsonp&&l.log("Using jsonp because this browser doesn't support CORS.")),l.contentType=i._.defaultContentType),l.withCredentials=c.withCredentials,l.ajaxDataType=c.jsonp?"jsonp":"text",e(l).bind(f.onStart,function(t,n){e.type(s)==="function"&&s.call(l),g.resolve(l)}),l._.initHandler=i.transports._logic.initHandler(l),p=function(n,s){var o=i._.error(r.noTransportOnInit);s=s||0;if(s>=n.length){s===0?l.log("No transports supported by the server were selected."):s===1?l.log("No fallback transports were selected."):l.log("Fallback transports exhausted."),e(l).triggerHandler(f.onError,[o]),g.reject(o),l.stop();return}if(l.state===i.connectionState.disconnected)return;var a=n[s],c=i.transports[a],v=function(){p(n,s+1)};l.transport=c;try{l._.initHandler.start(c,function(){var n=i._.firefoxMajorVersion(t.navigator.userAgent)>=11,r=!!l.withCredentials&&n;l.log("The start request succeeded. Transitioning to the connected state."),d(l)&&i.transports._logic.monitorKeepAlive(l),i.transports._logic.startHeartbeat(l),i._.configurePingInterval(l),h(l,i.connectionState.connecting,i.connectionState.connected)||l.log("WARNING! The connection was not in the connecting state."),l._.connectingMessageBuffer.drain(),e(l).triggerHandler(f.onStart),u.bind("unload",function(){l.log("Window unloading, stopping the connection."),l.stop(r)}),n&&u.bind("beforeunload",function(){t.setTimeout(function(){l.stop(r)},0)})},v)}catch(m){l.log(c.name+" transport threw '"+m.message+"' when attempting to start."),v()}};var b=l.url+"/negotiate",w=function(t,n){var s=i._.error(r.errorOnNegotiate,t,n._.negotiateRequest);e(n).triggerHandler(f.onError,s),g.reject(s),n.stop()};return e(l).triggerHandler(f.onStarting),b=i.transports._logic.prepareQueryString(l,b),l.log("Negotiating with '"+b+"'."),l._.negotiateRequest=i.transports._logic.ajax(l,{url:b,error:function(e,t){t!==a?w(e,l):g.reject(i._.error(r.stoppedWhileNegotiating,null,l._.negotiateRequest))},success:function(t){var n,s,o,u=[],a=[];try{n=l._parseResponse(t)}catch(h){w(i._.error(r.errorParsingNegotiateResponse,h),l);return}s=l._.keepAliveData,l.appRelativeUrl=n.Url,l.id=n.ConnectionId,l.token=n.ConnectionToken,l.webSocketServerUrl=n.WebSocketServerUrl,l._.pollTimeout=n.ConnectionTimeout*1e3+1e4,l.disconnectTimeout=n.DisconnectTimeout*1e3,l._.totalTransportConnectTimeout=l.transportConnectTimeout+n.TransportConnectTimeout*1e3,n.KeepAliveTimeout?(s.activated=!0,s.timeout=n.KeepAliveTimeout*1e3,s.timeoutWarning=s.timeout*l.keepAliveWarnAt,l._.beatInterval=(s.timeout-s.timeoutWarning)/3):s.activated=!1,l.reconnectWindow=l.disconnectTimeout+(s.timeout||0);if(!n.ProtocolVersion||n.ProtocolVersion!==l.clientProtocol){o=i._.error(i._.format(r.protocolIncompatible,l.clientProtocol,n.ProtocolVersion)),e(l).triggerHandler(f.onError,[o]),g.reject(o);return}e.each(i.transports,function(e){if(e.indexOf("_")===0||e==="webSockets"&&!n.TryWebSockets)return!0;a.push(e)}),e.isArray(c.transport)?e.each(c.transport,function(t,n){e.inArray(n,a)>=0&&u.push(n)}):c.transport==="auto"?u=a:e.inArray(c.transport,a)>=0&&u.push(c.transport),p(u)}}),g.promise()},starting:function(t){var n=this;return e(n).bind(f.onStarting,function(e,r){t.call(n)}),n},send:function(e){var t=this;if(t.state===i.connectionState.disconnected)throw new Error("SignalR: Connection must be started before data can be sent. Call .start() before .send()");if(t.state===i.connectionState.connecting)throw new Error("SignalR: Connection has not been fully initialized. Use .start().done() or .start().fail() to run logic after the connection has started.");return t.transport.send(t,e),t},received:function(t){var n=this;return e(n).bind(f.onReceived,function(e,r){t.call(n,r)}),n},stateChanged:function(t){var n=this;return e(n).bind(f.onStateChanged,function(e,r){t.call(n,r)}),n},error:function(t){var n=this;return e(n).bind(f.onError,function(e,r,i){n.lastError=r,t.call(n,r,i)}),n},disconnected:function(t){var n=this;return e(n).bind(f.onDisconnect,function(e,r){t.call(n)}),n},connectionSlow:function(t){var n=this;return e(n).bind(f.onConnectionSlow,function(e,r){t.call(n)}),n},reconnecting:function(t){var n=this;return e(n).bind(f.onReconnecting,function(e,r){t.call(n)}),n},reconnected:function(t){var n=this;return e(n).bind(f.onReconnect,function(e,r){t.call(n)}),n},stop:function(n,s){var l=this,c=l._deferral;l._.deferredStartHandler&&u.unbind("load",l._.deferredStartHandler),delete l._.config,delete l._.deferredStartHandler;if(!o&&(!l._.config||l._.config.waitForPageLoad===!0)){l.log("Stopping connection prior to negotiate."),c&&c.reject(i._.error(r.stoppedWhileLoading));return}if(l.state===i.connectionState.disconnected)return;return l.log("Stopping connection."),h(l,l.state,i.connectionState.disconnected),t.clearTimeout(l._.beatHandle),t.clearInterval(l._.pingIntervalId),l.transport&&(l.transport.stop(l),s!==!1&&l.transport.abort(l,n),d(l)&&i.transports._logic.stopMonitoringKeepAlive(l),l.transport=null),l._.negotiateRequest&&(l._.negotiateRequest.abort(a),delete l._.negotiateRequest),l._.initHandler&&l._.initHandler.stop(),e(l).triggerHandler(f.onDisconnect),delete l._deferral,delete l.messageId,delete l.groupsToken,delete l.id,delete l._.pingIntervalId,delete l._.lastMessageAt,delete l._.lastActiveAt,l._.connectingMessageBuffer.clear(),l},log:function(e){c(e,this.logging)}},i.fn.init.prototype=i.fn,i.noConflict=function(){return e.connection===i&&(e.connection=s),i},e.connection&&(s=e.connection),e.connection=e.signalR=i})(window.jQuery,window),function(e,t,n){function a(e){e._.keepAliveData.monitoring&&f(e),u.markActive(e)&&(e._.beatHandle=t.setTimeout(function(){a(e)},e._.beatInterval))}function f(t){var n=t._.keepAliveData,s;t.state===r.connectionState.connected&&(s=(new Date).getTime()-t._.lastMessageAt,s>=n.timeout?(t.log("Keep alive timed out.  Notifying transport that connection has been lost."),t.transport.lostConnection(t)):s>=n.timeoutWarning?n.userNotified||(t.log("Keep alive has been missed, connection may be dead/slow."),e(t).triggerHandler(i.onConnectionSlow),n.userNotified=!0):n.userNotified=!1)}function l(e,t){var n=e.url+t;return e.transport&&(n+="?transport="+e.transport.name),u.prepareQueryString(e,n)}function c(e){this.connection=e,this.startRequested=!1,this.startCompleted=!1,this.connectionStopped=!1}var r=e.signalR,i=e.signalR.events,s=e.signalR.changeState,o="__Start Aborted__",u;r.transports={},c.prototype={start:function(e,r,i){var s=this,o=s.connection,u=!1;if(s.startRequested||s.connectionStopped){o.log("WARNING! "+e.name+" transport cannot be started. Initialization ongoing or completed.");return}o.log(e.name+" transport starting."),s.transportTimeoutHandle=t.setTimeout(function(){u||(u=!0,o.log(e.name+" transport timed out when trying to connect."),s.transportFailed(e,n,i))},o._.totalTransportConnectTimeout),e.start(o,function(){u||s.initReceived(e,r)},function(t){return u||(u=!0,s.transportFailed(e,t,i)),!s.startCompleted||s.connectionStopped})},stop:function(){this.connectionStopped=!0,t.clearTimeout(this.transportTimeoutHandle),r.transports._logic.tryAbortStartRequest(this.connection)},initReceived:function(e,n){var i=this,s=i.connection;if(i.startRequested){s.log("WARNING! The client received multiple init messages.");return}if(i.connectionStopped)return;i.startRequested=!0,t.clearTimeout(i.transportTimeoutHandle),s.log(e.name+" transport connected. Initiating start request."),r.transports._logic.ajaxStart(s,function(){i.startCompleted=!0,n()})},transportFailed:function(n,s,o){var u=this.connection,a=u._deferral,f;if(this.connectionStopped)return;t.clearTimeout(this.transportTimeoutHandle),this.startRequested?this.startCompleted||(f=r._.error(r.resources.errorDuringStartRequest,s),u.log(n.name+" transport failed during the start request. Stopping the connection."),e(u).triggerHandler(i.onError,[f]),a&&a.reject(f),u.stop()):(n.stop(u),u.log(n.name+" transport failed to connect. Attempting to fall back."),o())}},u=r.transports._logic={ajax:function(t,n){return e.ajax(e.extend(!0,{},e.signalR.ajaxDefaults,{type:"GET",data:{},xhrFields:{withCredentials:t.withCredentials},contentType:t.contentType,dataType:t.ajaxDataType},n))},pingServer:function(t){var n,i,s=e.Deferred();return t.transport?(n=t.url+"/ping",n=u.addQs(n,t.qs),i=u.ajax(t,{url:n,success:function(e){var n;try{n=t._parseResponse(e)}catch(o){s.reject(r._.transportError(r.resources.pingServerFailedParse,t.transport,o,i)),t.stop();return}n.Response==="pong"?s.resolve():s.reject(r._.transportError(r._.format(r.resources.pingServerFailedInvalidResponse,e),t.transport,null,i))},error:function(e){e.status===401||e.status===403?(s.reject(r._.transportError(r._.format(r.resources.pingServerFailedStatusCode,e.status),t.transport,e,i)),t.stop()):s.reject(r._.transportError(r.resources.pingServerFailed,t.transport,e,i))}})):s.reject(r._.transportError(r.resources.noConnectionTransport,t.transport)),s.promise()},prepareQueryString:function(e,n){var r;return r=u.addQs(n,"clientProtocol="+e.clientProtocol),r=u.addQs(r,e.qs),e.token&&(r+="&connectionToken="+t.encodeURIComponent(e.token)),e.data&&(r+="&connectionData="+t.encodeURIComponent(e.data)),r},addQs:function(t,n){var r=t.indexOf("?")!==-1?"&":"?",i;if(!n)return t;if(typeof n=="object")return t+r+e.param(n);if(typeof n=="string"){i=n.charAt(0);if(i==="?"||i==="&")r="";return t+r+n}throw new Error("Query string property must be either a string or object.")},getUrl:function(e,n,r,i,s){var o=n==="webSockets"?"":e.baseUrl,a=o+e.appRelativeUrl,f="transport="+n;return!s&&e.groupsToken&&(f+="&groupsToken="+t.encodeURIComponent(e.groupsToken)),r?(i?a+="/poll":a+="/reconnect",!s&&e.messageId&&(f+="&messageId="+t.encodeURIComponent(e.messageId))):a+="/connect",a+="?"+f,a=u.prepareQueryString(e,a),s||(a+="&tid="+Math.floor(Math.random()*11)),a},maximizePersistentResponse:function(e){return{MessageId:e.C,Messages:e.M,Initialized:typeof e.S!="undefined"?!0:!1,ShouldReconnect:typeof e.T!="undefined"?!0:!1,LongPollDelay:e.L,GroupsToken:e.G}},updateGroups:function(e,t){t&&(e.groupsToken=t)},stringifySend:function(e,t){return typeof t=="string"||typeof t=="undefined"||t===null?t:e.json.stringify(t)},ajaxSend:function(t,n){var s=u.stringifySend(t,n),o=l(t,"/send"),a,f=function(t,s){e(s).triggerHandler(i.onError,[r._.transportError(r.resources.sendFailed,s.transport,t,a),n])};return a=u.ajax(t,{url:o,type:t.ajaxDataType==="jsonp"?"GET":"POST",contentType:r._.defaultContentType,data:{data:s},success:function(e){var n;if(e){try{n=t._parseResponse(e)}catch(r){f(r,t),t.stop();return}u.triggerReceived(t,n)}},error:function(e,n){if(n==="abort"||n==="parsererror")return;f(e,t)}}),a},ajaxAbort:function(e,t){if(typeof e.transport=="undefined")return;t=typeof t=="undefined"?!0:t;var n=l(e,"/abort");u.ajax(e,{url:n,async:t,timeout:1e3,type:"POST"}),e.log("Fired ajax abort async = "+t+".")},ajaxStart:function(t,n){var s=function(e){var n=t._deferral;n&&n.reject(e)},a=function(n){t.log("The start request failed. Stopping the connection."),e(t).triggerHandler(i.onError,[n]),s(n),t.stop()};t._.startRequest=u.ajax(t,{url:l(t,"/start"),success:function(e,i,s){var o;try{o=t._parseResponse(e)}catch(u){a(r._.error(r._.format(r.resources.errorParsingStartResponse,e),u,s));return}o.Response==="started"?n():a(r._.error(r._.format(r.resources.invalidStartResponse,e),null,s))},error:function(e,n,i){n!==o?a(r._.error(r.resources.errorDuringStartRequest,i,e)):(t.log("The start request aborted because connection.stop() was called."),s(r._.error(r.resources.stoppedDuringStartRequest,null,e)))}})},tryAbortStartRequest:function(e){e._.startRequest&&(e._.startRequest.abort(o),delete e._.startRequest)},tryInitialize:function(e,t){e.Initialized&&t()},triggerReceived:function(t,n){t._.connectingMessageBuffer.tryBuffer(n)||e(t).triggerHandler(i.onReceived,[n])},processMessages:function(t,n,r){var i;u.markLastMessage(t),n&&(i=u.maximizePersistentResponse(n),u.updateGroups(t,i.GroupsToken),i.MessageId&&(t.messageId=i.MessageId),i.Messages&&(e.each(i.Messages,function(e,n){u.triggerReceived(t,n)}),u.tryInitialize(i,r)))},monitorKeepAlive:function(t){var n=t._.keepAliveData;n.monitoring?t.log("Tried to monitor keep alive but it's already being monitored."):(n.monitoring=!0,u.markLastMessage(t),t._.keepAliveData.reconnectKeepAliveUpdate=function(){u.markLastMessage(t)},e(t).bind(i.onReconnect,t._.keepAliveData.reconnectKeepAliveUpdate),t.log("Now monitoring keep alive with a warning timeout of "+n.timeoutWarning+", keep alive timeout of "+n.timeout+" and disconnecting timeout of "+t.disconnectTimeout))},stopMonitoringKeepAlive:function(t){var n=t._.keepAliveData;n.monitoring&&(n.monitoring=!1,e(t).unbind(i.onReconnect,t._.keepAliveData.reconnectKeepAliveUpdate),t._.keepAliveData={},t.log("Stopping the monitoring of the keep alive."))},startHeartbeat:function(e){e._.lastActiveAt=(new Date).getTime(),a(e)},markLastMessage:function(e){e._.lastMessageAt=(new Date).getTime()},markActive:function(e){return u.verifyLastActive(e)?(e._.lastActiveAt=(new Date).getTime(),!0):!1},isConnectedOrReconnecting:function(e){return e.state===r.connectionState.connected||e.state===r.connectionState.reconnecting},ensureReconnectingState:function(t){return s(t,r.connectionState.connected,r.connectionState.reconnecting)===!0&&e(t).triggerHandler(i.onReconnecting),t.state===r.connectionState.reconnecting},clearReconnectTimeout:function(e){e&&e._.reconnectTimeout&&(t.clearTimeout(e._.reconnectTimeout),delete e._.reconnectTimeout)},verifyLastActive:function(t){if((new Date).getTime()-t._.lastActiveAt>=t.reconnectWindow){var n=r._.format(r.resources.reconnectWindowTimeout,new Date(t._.lastActiveAt),t.reconnectWindow);return t.log(n),e(t).triggerHandler(i.onError,[r._.error(n,"TimeoutException")]),t.stop(!1,!1),!1}return!0},reconnect:function(e,n){var i=r.transports[n];if(u.isConnectedOrReconnecting(e)&&!e._.reconnectTimeout){if(!u.verifyLastActive(e))return;e._.reconnectTimeout=t.setTimeout(function(){if(!u.verifyLastActive(e))return;i.stop(e),u.ensureReconnectingState(e)&&(e.log(n+" reconnecting."),i.start(e))},e.reconnectDelay)}},handleParseFailure:function(t,n,s,o,u){var a=r._.transportError(r._.format(r.resources.parseFailed,n),t.transport,s,u);o&&o(a)?t.log("Failed to parse server response while attempting to connect."):(e(t).triggerHandler(i.onError,[a]),t.stop())},initHandler:function(e){return new c(e)},foreverFrame:{count:0,connections:{}}}}(window.jQuery,window),function(e,t,n){var r=e.signalR,i=e.signalR.events,s=e.signalR.changeState,o=r.transports._logic;r.transports.webSockets={name:"webSockets",supportsKeepAlive:function(){return!0},send:function(t,n){var s=o.stringifySend(t,n);try{t.socket.send(s)}catch(u){e(t).triggerHandler(i.onError,[r._.transportError(r.resources.webSocketsInvalidState,t.transport,u,t.socket),n])}},start:function(n,u,a){var f,l=!1,c=this,h=!u,p=e(n);if(!t.WebSocket){a();return}n.socket||(n.webSocketServerUrl?f=n.webSocketServerUrl:f=n.wsProtocol+n.host,f+=o.getUrl(n,this.name,h),n.log("Connecting to websocket endpoint '"+f+"'."),n.socket=new t.WebSocket(f),n.socket.onopen=function(){l=!0,n.log("Websocket opened."),o.clearReconnectTimeout(n),s(n,r.connectionState.reconnecting,r.connectionState.connected)===!0&&p.triggerHandler(i.onReconnect)},n.socket.onclose=function(t){var s;if(this===n.socket){l&&typeof t.wasClean!="undefined"&&t.wasClean===!1?(s=r._.transportError(r.resources.webSocketClosed,n.transport,t),n.log("Unclean disconnect from websocket: "+(t.reason||"[no reason given]."))):n.log("Websocket closed.");if(!a||!a(s))s&&e(n).triggerHandler(i.onError,[s]),c.reconnect(n)}},n.socket.onmessage=function(t){var r;try{r=n._parseResponse(t.data)}catch(i){o.handleParseFailure(n,t.data,i,a,t);return}r&&(e.isEmptyObject(r)||r.M?o.processMessages(n,r,u):o.triggerReceived(n,r))})},reconnect:function(e){o.reconnect(e,this.name)},lostConnection:function(e){this.reconnect(e)},stop:function(e){o.clearReconnectTimeout(e),e.socket&&(e.log("Closing the Websocket."),e.socket.close(),e.socket=null)},abort:function(e,t){o.ajaxAbort(e,t)}}}(window.jQuery,window),function(e,t,n){var r=e.signalR,i=e.signalR.events,s=e.signalR.changeState,o=r.transports._logic,u=function(e){t.clearTimeout(e._.reconnectAttemptTimeoutHandle),delete e._.reconnectAttemptTimeoutHandle};r.transports.serverSentEvents={name:"serverSentEvents",supportsKeepAlive:function(){return!0},timeOut:3e3,start:function(n,a,f){var l=this,c=!1,h=e(n),p=!a,d;n.eventSource&&(n.log("The connection already has an event source. Stopping it."),n.stop());if(!t.EventSource){f&&(n.log("This browser doesn't support SSE."),f());return}d=o.getUrl(n,this.name,p);try{n.log("Attempting to connect to SSE endpoint '"+d+"'."),n.eventSource=new t.EventSource(d,{withCredentials:n.withCredentials})}catch(v){n.log("EventSource failed trying to connect with error "+v.Message+"."),f?f():(h.triggerHandler(i.onError,[r._.transportError(r.resources.eventSourceFailedToConnect,n.transport,v)]),p&&l.reconnect(n));return}p&&(n._.reconnectAttemptTimeoutHandle=t.setTimeout(function(){c===!1&&n.eventSource.readyState!==t.EventSource.OPEN&&l.reconnect(n)},l.timeOut)),n.eventSource.addEventListener("open",function(e){n.log("EventSource connected."),u(n),o.clearReconnectTimeout(n),c===!1&&(c=!0,s(n,r.connectionState.reconnecting,r.connectionState.connected)===!0&&h.triggerHandler(i.onReconnect))},!1),n.eventSource.addEventListener("message",function(e){var t;if(e.data==="initialized")return;try{t=n._parseResponse(e.data)}catch(r){o.handleParseFailure(n,e.data,r,f,e);return}o.processMessages(n,t,a)},!1),n.eventSource.addEventListener("error",function(e){var s=r._.transportError(r.resources.eventSourceError,n.transport,e);if(this!==n.eventSource)return;if(f&&f(s))return;n.log("EventSource readyState: "+n.eventSource.readyState+"."),e.eventPhase===t.EventSource.CLOSED?(n.log("EventSource reconnecting due to the server connection ending."),l.reconnect(n)):(n.log("EventSource error."),h.triggerHandler(i.onError,[s]))},!1)},reconnect:function(e){o.reconnect(e,this.name)},lostConnection:function(e){this.reconnect(e)},send:function(e,t){o.ajaxSend(e,t)},stop:function(e){u(e),o.clearReconnectTimeout(e),e&&e.eventSource&&(e.log("EventSource calling close()."),e.eventSource.close(),e.eventSource=null,delete e.eventSource)},abort:function(e,t){o.ajaxAbort(e,t)}}}(window.jQuery,window),function(e,t,n){var r=e.signalR,i=e.signalR.events,s=e.signalR.changeState,o=r.transports._logic,u=function(){var e=t.document.createElement("iframe");return e.setAttribute("style","position:absolute;top:0;left:0;width:0;height:0;visibility:hidden;"),e},a=function(){var e=null,n=1e3,i=0;return{prevent:function(){r._.ieVersion<=8&&(i===0&&(e=t.setInterval(function(){var e=u();t.document.body.appendChild(e),t.document.body.removeChild(e),e=null},n)),i++)},cancel:function(){i===1&&t.clearInterval(e),i>0&&i--}}}();r.transports.foreverFrame={name:"foreverFrame",supportsKeepAlive:function(){return!0},iframeClearThreshold:50,start:function(e,n,r){var i=this,s=o.foreverFrame.count+=1,f,l=u(),c=function(){e.log("Forever frame iframe finished loading and is no longer receiving messages."),(!r||!r())&&i.reconnect(e)};if(t.EventSource){r&&(e.log("Forever Frame is not supported by SignalR on browsers with SSE support."),r());return}l.setAttribute("data-signalr-connection-id",e.id),a.prevent(),f=o.getUrl(e,this.name),f+="&frameId="+s,t.document.documentElement.appendChild(l),e.log("Binding to iframe's load event."),l.addEventListener?l.addEventListener("load",c,!1):l.attachEvent&&l.attachEvent("onload",c),l.src=f,o.foreverFrame.connections[s]=e,e.frame=l,e.frameId=s,n&&(e.onSuccess=function(){e.log("Iframe transport started."),n()})},reconnect:function(e){var n=this;o.isConnectedOrReconnecting(e)&&o.verifyLastActive(e)&&t.setTimeout(function(){if(!o.verifyLastActive(e))return;if(e.frame&&o.ensureReconnectingState(e)){var t=e.frame,r=o.getUrl(e,n.name,!0)+"&frameId="+e.frameId;e.log("Updating iframe src to '"+r+"'."),t.src=r}},e.reconnectDelay)},lostConnection:function(e){this.reconnect(e)},send:function(e,t){o.ajaxSend(e,t)},receive:function(t,n){var i,s,u;t.json!==t._originalJson&&(n=t._originalJson.stringify(n)),u=t._parseResponse(n),o.processMessages(t,u,t.onSuccess);if(t.state===e.signalR.connectionState.connected){t.frameMessageCount=(t.frameMessageCount||0)+1;if(t.frameMessageCount>r.transports.foreverFrame.iframeClearThreshold){t.frameMessageCount=0,i=t.frame.contentWindow||t.frame.contentDocument;if(i&&i.document&&i.document.body){s=i.document.body;while(s.firstChild)s.removeChild(s.firstChild)}}}},stop:function(e){var n=null;a.cancel();if(e.frame){if(e.frame.stop)e.frame.stop();else try{n=e.frame.contentWindow||e.frame.contentDocument,n.document&&n.document.execCommand&&n.document.execCommand("Stop")}catch(r){e.log("Error occured when stopping foreverFrame transport. Message = "+r.message+".")}e.frame.parentNode===t.document.body&&t.document.body.removeChild(e.frame),delete o.foreverFrame.connections[e.frameId],e.frame=null,e.frameId=null,delete e.frame,delete e.frameId,delete e.onSuccess,delete e.frameMessageCount,e.log("Stopping forever frame.")}},abort:function(e,t){o.ajaxAbort(e,t)},getConnection:function(e){return o.foreverFrame.connections[e]},started:function(t){s(t,r.connectionState.reconnecting,r.connectionState.connected)===!0&&e(t).triggerHandler(i.onReconnect)}}}(window.jQuery,window),function(e,t,n){var r=e.signalR,i=e.signalR.events,s=e.signalR.changeState,o=e.signalR.isDisconnecting,u=r.transports._logic;r.transports.longPolling={name:"longPolling",supportsKeepAlive:function(){return!1},reconnectDelay:3e3,start:function(n,a,f){var l=this,c=function(){c=e.noop,n.log("LongPolling connected."),a()},h=function(e){return f(e)?(n.log("LongPolling failed to connect."),!0):!1},p=n._,d=0,v=function(n){t.clearTimeout(p.reconnectTimeoutId),p.reconnectTimeoutId=null,s(n,r.connectionState.reconnecting,r.connectionState.connected)===!0&&(n.log("Raising the reconnect event"),e(n).triggerHandler(i.onReconnect))},m=36e5;n.pollXhr&&(n.log("Polling xhr requests already exists, aborting."),n.stop()),n.messageId=null,p.reconnectTimeoutId=null,p.pollTimeoutId=t.setTimeout(function(){(function s(a,f){var g=a.messageId,y=g===null,b=!y,w=!f,E=u.getUrl(a,l.name,b,w,!0),S={};a.messageId&&(S.messageId=a.messageId),a.groupsToken&&(S.groupsToken=a.groupsToken);if(o(a)===!0)return;n.log("Opening long polling request to '"+E+"'."),a.pollXhr=u.ajax(n,{xhrFields:{onprogress:function(){u.markLastMessage(n)}},url:E,type:"POST",contentType:r._.defaultContentType,data:S,timeout:n._.pollTimeout,success:function(r){var i,f=0,l,m;n.log("Long poll complete."),d=0;try{i=n._parseResponse(r)}catch(g){u.handleParseFailure(a,r,g,h,a.pollXhr);return}p.reconnectTimeoutId!==null&&v(a),i&&(l=u.maximizePersistentResponse(i)),u.processMessages(a,i,c),l&&e.type(l.LongPollDelay)==="number"&&(f=l.LongPollDelay);if(o(a)===!0)return;m=l&&l.ShouldReconnect;if(m&&!u.ensureReconnectingState(a))return;f>0?p.pollTimeoutId=t.setTimeout(function(){s(a,m)},f):s(a,m)},error:function(o,f){var c=r._.transportError(r.resources.longPollFailed,n.transport,o,a.pollXhr);t.clearTimeout(p.reconnectTimeoutId),p.reconnectTimeoutId=null;if(f==="abort"){n.log("Aborted xhr request.");return}if(!h(c)){d++,n.state!==r.connectionState.reconnecting&&(n.log("An error occurred using longPolling. Status = "+f+".  Response = "+o.responseText+"."),e(a).triggerHandler(i.onError,[c]));if((n.state===r.connectionState.connected||n.state===r.connectionState.reconnecting)&&!u.verifyLastActive(n))return;if(!u.ensureReconnectingState(a))return;p.pollTimeoutId=t.setTimeout(function(){s(a,!0)},l.reconnectDelay)}}}),b&&f===!0&&(p.reconnectTimeoutId=t.setTimeout(function(){v(a)},Math.min(1e3*(Math.pow(2,d)-1),m)))})(n)},250)},lostConnection:function(e){e.pollXhr&&e.pollXhr.abort("lostConnection")},send:function(e,t){u.ajaxSend(e,t)},stop:function(e){t.clearTimeout(e._.pollTimeoutId),t.clearTimeout(e._.reconnectTimeoutId),delete e._.pollTimeoutId,delete e._.reconnectTimeoutId,e.pollXhr&&(e.pollXhr.abort(),e.pollXhr=null,delete e.pollXhr)},abort:function(e,t){u.ajaxAbort(e,t)}}}(window.jQuery,window),function(e,t,n){function s(e){return e+r}function o(e,t,n){var r,i=e.length,s=[];for(r=0;r<i;r+=1)e.hasOwnProperty(r)&&(s[r]=t.call(n,e[r],r,e));return s}function u(t){return e.isFunction(t)?null:e.type(t)==="undefined"?null:t}function a(e){for(var t in e)if(e.hasOwnProperty(t))return!0;return!1}function f(e,t){var n=e._.invocationCallbacks,r;a(n)&&e.log("Clearing hub invocation callbacks with error: "+t+"."),e._.invocationCallbackId=0,delete e._.invocationCallbacks,e._.invocationCallbacks={};for(var i in n)r=n[i],r.method.call(r.scope,{E:t})}function l(e,t){return new l.fn.init(e,t)}function c(t,n){var r={qs:null,logging:!1,useDefaultPath:!0};e.extend(r,n);if(!t||r.useDefaultPath)t=(t||"")+"/signalr";return new c.fn.init(t,r)}var r=".hubProxy",i=e.signalR;l.fn=l.prototype={init:function(e,t){this.state={},this.connection=e,this.hubName=t,this._={callbackMap:{}}},constructor:l,hasSubscriptions:function(){return a(this._.callbackMap)},on:function(t,n){var r=this,i=r._.callbackMap;return t=t.toLowerCase(),i[t]||(i[t]={}),i[t][n]=function(e,t){n.apply(r,t)},e(r).bind(s(t),i[t][n]),r},off:function(t,n){var r=this,i=r._.callbackMap,o;return t=t.toLowerCase(),o=i[t],o&&(o[n]?(e(r).unbind(s(t),o[n]),delete o[n],a(o)||delete i[t]):n||(e(r).unbind(s(t)),delete i[t])),r},invoke:function(t){var n=this,r=n.connection,s=e.makeArray(arguments).slice(1),a=o(s,u),f={H:n.hubName,M:t,A:a,I:r._.invocationCallbackId},l=e.Deferred(),c=function(s){var o=n._maximizeHubResponse(s),u,a;e.extend(n.state,o.State),o.Progress?l.notifyWith?l.notifyWith(n,[o.Progress.Data]):r._.progressjQueryVersionLogged||(r.log("A hub method invocation progress update was received but the version of jQuery in use ("+e.prototype.jquery+") does not support progress updates. Upgrade to jQuery 1.7+ to receive progress notifications."),r._.progressjQueryVersionLogged=!0):o.Error?(o.StackTrace&&r.log(o.Error+"\n"+o.StackTrace+"."),u=o.IsHubException?"HubException":"Exception",a=i._.error(o.Error,u),a.data=o.ErrorData,r.log(n.hubName+"."+t+" failed to execute. Error: "+a.message),l.rejectWith(n,[a])):(r.log("Invoked "+n.hubName+"."+t),l.resolveWith(n,[o.Result]))};return r._.invocationCallbacks[r._.invocationCallbackId.toString()]={scope:n,method:c},r._.invocationCallbackId+=1,e.isEmptyObject(n.state)||(f.S=n.state),r.log("Invoking "+n.hubName+"."+t),r.send(f),l.promise()},_maximizeHubResponse:function(e){return{State:e.S,Result:e.R,Progress:e.P?{Id:e.P.I,Data:e.P.D}:null,Id:e.I,IsHubException:e.H,Error:e.E,StackTrace:e.T,ErrorData:e.D}}},l.fn.init.prototype=l.fn,c.fn=c.prototype=e.connection(),c.fn.init=function(t,n){var r={qs:null,logging:!1,useDefaultPath:!0},i=this;e.extend(r,n),e.signalR.fn.init.call(i,t,r.qs,r.logging),i.proxies={},i._.invocationCallbackId=0,i._.invocationCallbacks={},i.received(function(t){var n,r,o,u,a,f;if(!t)return;typeof t.P!="undefined"?(o=t.P.I.toString(),u=i._.invocationCallbacks[o],u&&u.method.call(u.scope,t)):typeof t.I!="undefined"?(o=t.I.toString(),u=i._.invocationCallbacks[o],u&&(i._.invocationCallbacks[o]=null,delete i._.invocationCallbacks[o],u.method.call(u.scope,t))):(n=this._maximizeClientHubInvocation(t),i.log("Triggering client hub event '"+n.Method+"' on hub '"+n.Hub+"'."),a=n.Hub.toLowerCase(),f=n.Method.toLowerCase(),r=this.proxies[a],e.extend(r.state,n.State),e(r).triggerHandler(s(f),[n.Args]))}),i.error(function(e,t){var n,r;if(!t)return;n=t.I,r=i._.invocationCallbacks[n],r&&(i._.invocationCallbacks[n]=null,delete i._.invocationCallbacks[n],r.method.call(r.scope,{E:e}))}),i.reconnecting(function(){i.transport&&i.transport.name==="webSockets"&&f(i,"Connection started reconnecting before invocation result was received.")}),i.disconnected(function(){f(i,"Connection was disconnected before invocation result was received.")})},c.fn._maximizeClientHubInvocation=function(e){return{Hub:e.H,Method:e.M,Args:e.A,State:e.S}},c.fn._registerSubscribedHubs=function(){var t=this;t._subscribedToHubs||(t._subscribedToHubs=!0,t.starting(function(){var n=[];e.each(t.proxies,function(e){this.hasSubscriptions()&&(n.push({name:e}),t.log("Client subscribed to hub '"+e+"'."))}),n.length===0&&t.log("No hubs have been subscribed to.  The client will not receive data from hubs.  To fix, declare at least one client side function prior to connection start for each hub you wish to subscribe to."),t.data=t.json.stringify(n)}))},c.fn.createHubProxy=function(e){e=e.toLowerCase();var t=this.proxies[e];return t||(t=l(this,e),this.proxies[e]=t),this._registerSubscribedHubs(),t},c.fn.init.prototype=c.fn,e.hubConnection=c}(window.jQuery,window),function(e,t){e.signalR.version="2.2.0"}(window.jQuery);