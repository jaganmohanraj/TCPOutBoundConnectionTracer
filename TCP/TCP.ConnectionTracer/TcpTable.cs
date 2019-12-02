using System.Collections;
using System.Collections.Generic;

namespace TCP.OutBoundConnection
{
    internal class TcpTable : IEnumerable<TcpRow>
    {
        #region Private Fields

        #endregion

        #region Constructors

        public TcpTable(IEnumerable<TcpRow> tcpRows)
        {
            Rows = tcpRows;
        }

        #endregion

        #region Public Properties

        public IEnumerable<TcpRow> Rows { get; }

        #endregion

        #region IEnumerable<TcpRow> Members

        public IEnumerator<TcpRow> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        #endregion
    }
}