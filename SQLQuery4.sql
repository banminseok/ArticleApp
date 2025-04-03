select year(created) as year,  month(created) as month, count(*) as count
from Notices
where created >= dateadd(month, -12, getdate())
and created <= getdate()
group by year(created), month(created)
order by year(created), month(created)