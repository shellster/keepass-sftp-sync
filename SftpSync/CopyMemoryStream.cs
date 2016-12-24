using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SftpSync
{
    /// <summary>
    /// Alloved geting buffer array from MemoryStream, before it closied    
    /// </summary>
    /// /// Taking from src of plugin IOProtocolExt Copyright (C) 2011-2016 Dominik Reichl <dominik.reichl@t-online.de>
    public sealed class CopyMemoryStream : MemoryStream
    {
        private List<byte> m_lCopyBuffer;

        public CopyMemoryStream(List<byte> lCopyBuffer) : base()
        {
            m_lCopyBuffer = lCopyBuffer;
        }

        public override void Close()
        {
            if (m_lCopyBuffer != null)
            {
                m_lCopyBuffer.AddRange(this.ToArray());
                m_lCopyBuffer = null; // Copy once only
            }

            base.Close();
        }
    }

}
