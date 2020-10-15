using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static OracleDbHelper.Utils.OracleHelper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleDbHelperTest
{
    public class OracleHelperTests
    {
        [Fact]
        public void OpenConnTest()
        {
            //given
            //when
            using (OracleConnection conn = OpenConn())
            {
                //then
                Assert.True(conn.State == ConnectionState.Open);
            }
        }

        [Fact]
        public void ReadTableTest()
        {
            //given
            string querySql = "select t.* from yw_xjzl t where rownum < 100";
            OracleConnection conn = OpenConn();
            //when
            DataTable dt = ReadTable(querySql, null, conn);
            //then
            Assert.True(dt.Rows.Count > 0);
            conn.Close();
        }

        [Fact]
        public void ExecuteSqlUpdateTest()
        {
            //given
            string updSql = @"update jc_spzl t set t.sf_tdyp = :viSfTdyp where t.shangp_id = :viShangpId";
            OracleParameter[] param = new OracleParameter[]
            {
                AddInputParameter("viSfTdyp", "Y", OracleDbType.Varchar2,2),
                AddInputParameter("viShangpId", "SPH11117186", OracleDbType.Varchar2, 50)
            };
            OracleConnection conn = OpenConn();
            int affectRows = 0;
            //when
            try
            {
                affectRows = ExecuteSql(updSql, param, conn);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message.Length > 0);
            }
            //then
            finally
            {
                Assert.True(affectRows > 0);
                conn.Close();
            }
        }

        [Fact]
        public void ExecuteSqlDelTest()
        {
            //given
            string delSql = @"delete jc_zdwh_mx t where t.english_name = :viEnglishName and t.hanghao = :viHangHao";
            OracleParameter[] param = new OracleParameter[]
            {
                AddInputParameter("viEnglishName", "YAOP_CATEGORY", OracleDbType.Varchar2, 50),
                AddInputParameter("viHangHao", "28", OracleDbType.Varchar2, 50)
            };
            OracleConnection conn = OpenConn();
            int affectRows = 0;
            //when
            try
            {
                affectRows = ExecuteSql(delSql, param, conn);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message.Length > 0);
            }
            //then
            finally
            {
                Assert.True(affectRows >= 0);
                conn.Close();
            }
        }

        [Fact]
        public void ExcecuteSqlInsTest()
        {
            //given
            string insSql = @"insert into jc_zdwh_mx (ENGLISH_NAME,   HANGHAO,    ZHID_CONTENT,   ZHIDZ)
                                              values (:viEnglishName, :viHangHao, :viZhidContent, :viZhidZ)";
            OracleParameter[] param = new OracleParameter[]
            {
                AddInputParameter("viEnglishName", "YAOP_CATEGORY", OracleDbType.Varchar2, 50),
                AddInputParameter("viHangHao", "28", OracleDbType.Varchar2, 50),
                AddInputParameter("viZhidContent", "²è¾ÆË®", OracleDbType.Varchar2, 50),
                AddInputParameter("viZhidZ", "28", OracleDbType.Varchar2, 50)
            };
            OracleConnection conn = OpenConn();
            int affectRows = 0;
            //when
            try
            {
                affectRows = ExecuteSql(insSql, param, conn);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message.Length > 0);
            }
            //then
            finally
            {
                Assert.True(affectRows > 0);
                conn.Close();
            }
        }

        [Fact]
        public void ExecuteProcNormalTest()
        {
            //given
            string procedureName = "PRC_UTL_GETSEQNO";
            OracleParameter[] param = new OracleParameter[]
            {
                AddInputParameter("iv_type", "DWI", OracleDbType.Varchar2, 50),
                AddInputParameter("iv_commit", "Y", OracleDbType.Varchar2, 50),
                AddOutputParameter("ov_seqno", OracleDbType.Varchar2, 50),
                AddInputParameter("iv_house_id", "CK000000001", OracleDbType.Varchar2, 50)
            };
            OracleConnection conn = OpenConn();
            //when
            ExecuteProc(procedureName, param, conn);
            //then
            Assert.True(param[2].Value != null);
        }

        [Fact]
        public void ExecuteFunctionTest()
        {
            //given
            string fncName = "FNC_GET_SEQNO";
            OracleParameter[] param = new OracleParameter[]
            {
                AddInputParameter("iv_djlx", "DWI", OracleDbType.Varchar2, 50),
                AddInputParameter("iv_house_id", "CK000000001", OracleDbType.Varchar2, 50),
                AddReturnValue("rvNewDwId", OracleDbType.Varchar2, 50)
            };
            OracleConnection conn = OpenConn();
            //when
            ExecuteProc(fncName, param, conn);
            //then
            Assert.True(param[2] != null);
        }
    }
}
