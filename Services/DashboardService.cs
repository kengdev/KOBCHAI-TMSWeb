using TMSWeb.Models.ViewModels;
using TMSWeb.Repositories;

public class DashboardService
{
    private readonly DashboardRepository _dashboardRepository;
    private readonly DropListRepository _dropListRepository;
    private readonly CustomerRepository _customerRepository;

    public DashboardService(
        DashboardRepository dashboardRepository,
        DropListRepository dropListRepository,
        CustomerRepository customerRepository)
    {
        _dashboardRepository = dashboardRepository;
        _dropListRepository = dropListRepository;
        _customerRepository = customerRepository;
    }

    public List<DropDownViewModel> DropListMonth()
    {
        return _dropListRepository.GetMonth();
    }

    public List<DropDownViewModel> DropListYear()
    {
        return _dropListRepository.GetYear();
    }
}