export default class{constructor(t){this.bufferSize=t.bufferSize||4096,this.sampleRate=t.sampleRate,this.samples=t.samples}finish(){this._joinSamples();const t=new ArrayBuffer(44+2*this.samples.length),e=new DataView(t);this._writeString(e,0,"RIFF"),e.setUint32(4,36+2*this.samples.length,!0),this._writeString(e,8,"WAVE"),this._writeString(e,12,"fmt "),e.setUint32(16,16,!0),e.setUint16(20,1,!0),e.setUint16(22,1,!0),e.setUint32(24,this.sampleRate,!0),e.setUint32(28,4*this.sampleRate,!0),e.setUint16(32,4,!0),e.setUint16(34,16,!0),this._writeString(e,36,"data"),e.setUint32(40,2*this.samples.length,!0),this._floatTo16BitPCM(e,44,this.samples);const s=new Blob([e],{type:"audio/wav"});return{id:Date.now(),blob:s,url:URL.createObjectURL(s)}}_floatTo16BitPCM(t,e,s){for(let i=0;i<s.length;i++,e+=2){const n=Math.max(-1,Math.min(1,s[i]));t.setInt16(e,n<0?32768*n:32767*n,!0)}}_joinSamples(){const t=this.samples.length*this.bufferSize,e=new Float64Array(t);let s=0;for(let t=0;t<this.samples.length;t++){const i=this.samples[t];e.set(i,s),s+=i.length}this.samples=e}_writeString(t,e,s){for(let i=0;i<s.length;i++)t.setUint8(e+i,s.charCodeAt(i))}}
