using System;
using System.Windows.Forms;

/// #############################################################################################################################################
/// THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
/// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL CASEY CARDINIA LIBRARIES BE LIABLE FOR ANY DIRECT, 
/// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
/// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
/// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
/// POSSIBILITY OF SUCH DAMAGE.
/// 
/// Program name      : CCLKiosk.exe
/// Author            : Lucas Baker
/// Company           : Casey Cardinia Libraries
/// Date created      : 30-08-2018
/// Purpose           : Interface to switch between kiosk applications on a locked down PC to help prevent access to desktop/unwanted software
///                     without the use of the typical Windows interface. Works with both touch and non-touch screens in landscape and portrait
///                     mode and is easily configurable.
///
/// Revision History  :
///
/// Date        Author          Ref    Revision (Date in YYYYMMDD format) 
/// 
/// #############################################################################################################################################

namespace CCLKiosk
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HomeForm());
        }
    }
}
